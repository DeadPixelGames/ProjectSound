using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** <summary>
    Singleton component controlling the item inventory.
    </summary>
*/
public class Inventory : MonoBehaviour {
    #region Singleton
    public static Inventory instance;

    private static void InitSingleton(Inventory thisInstance) {
        if(instance != null && instance != thisInstance) {
            throw new System.Exception("Hay al menos dos instancias de " + thisInstance.GetType().Name);
        } else {
            instance = thisInstance;
        }
    }
    #endregion


    //Constant inputs for DEBUG
    private const string ADD_DEFAULT_ITEM = "r";
    private const string REMOVE_DEFAULT_ITEM = "y";
    private const string ADD_ITEM_SLOT = "i";
    private const string REMOVE_ITEM_SLOT = "p";
    private const string ACTIVATE_DEBUG = "m";

    /*
     * <summary>
     * Reference to slot unselected background texture
     * </summary>
     * */
    [SerializeField]
    private Sprite[] unselectedBackground;

    /* <summary>
     * Reference to slot selected background texture
     * </summary>
     */
    [SerializeField]
    private Sprite[] selectedBackground;

    /** <summary>
        Reference to the HUD area displaying the inventory, so it can be updated.
        </summary>
    */
    [SerializeField]
    private GameObject inventoryUI;


    /** <summary>
        Reference to the HUD slot of an item in the inventory.
        </summary>
    */
    [SerializeField]
    private GameObject inventorySlot;

    /** <summary>
        Maximum amount of items that can be held at any given time.
        </summary>
    */
   [SerializeField]
    private int maxItems;
    /** <summary>
        Test Bubble. For Debug purposes only
        </summary>
    */
    [SerializeField]
    private Item[] bubble;

    /** <summary>
        Index of the active item in the list, or `null` if no item is selected.
        </summary>
    */
    private int activeItemIndex;

    /** <summary>
        List of items held in the inventory.
        </summary>
    */
    private List<Item> items;

    private bool debugActive = false;

    #region Unity
    void Awake() {
        Inventory.InitSingleton(this);
        // No item selected at first
        this.activeItemIndex = 0;
        this.items = new List<Item>();
        for(int i = 0; i < maxItems; i++)
        {
            //TODO Set this to null when we are done testing
            this.items.Add(null);
        }
        Refresh();
        
    }
    //For Debug Purposes Only
    void Update()
    {
        // DEBUG CONTROLS
        if (debugActive)
        {
            if (Input.GetKeyDown(ADD_DEFAULT_ITEM))
            {
                AddItem(bubble[activeItemIndex]);
            
            }
            if (Input.GetKeyDown(REMOVE_DEFAULT_ITEM))
            {
                RemoveActiveItem();
            }
            if (Input.GetKeyDown(REMOVE_ITEM_SLOT))
            {
                SetMaxItems(GetMaxItems() - 1);
            }
            if (Input.GetKeyDown(ADD_ITEM_SLOT))
            {
                SetMaxItems(GetMaxItems() + 1);
            }
        }
        if (Input.GetKeyDown(ACTIVATE_DEBUG))
        {
            debugActive = !debugActive;
        }
        
    }
    #endregion

    /** <summary>
        Updates the HUD so it displays the current state of the inventory.
        </summary>
    */
    private void Refresh() {
        List<GameObject> children = new List<GameObject>();
        //Initiate the children list with the invetory's children gameObjects
        for(int i = 0; i < this.inventoryUI.transform.childCount; i++)
        {
            children.Add(this.inventoryUI.transform.GetChild(i).gameObject);
        }
        //In case the inventory's children are more than the item count the surplus objects are destroyed
        if(inventoryUI.transform.childCount > this.items.Count)
        {
            for(int i = this.items.Count; i < inventoryUI.transform.childCount; i++)
            {
                Destroy(children[i].gameObject);
            }
        }
        //In case the inventory's children are less than the item count the remaining objects are added
        else if(inventoryUI.transform.childCount < this.items.Count)
        {
            for(int i = inventoryUI.transform.childCount; i < this.items.Count; i++)
            {
                GameObject slot = GameObject.Instantiate(inventorySlot);
                slot.transform.SetParent(inventoryUI.transform);
                //The scale is set to 1 because if not when more objects are added the scale gets bigger (?)
                slot.transform.localScale = new Vector3(1, 1, 1);
                //Set the Id of the Slot the same as the index of the Item in the List
                slot.GetComponent<InventoryButtonController>().id = i;

            }
        }
        children = new List<GameObject>();
        //Resets the list with the new GameObjects, just in case some of them are destroyed or some are added
        for (int i = 0; i < this.inventoryUI.transform.childCount; i++)
        {
            children.Add(this.inventoryUI.transform.GetChild(i).gameObject);
            
        }
        //Update the visuals of the inventory
        for (int i = 0; i < this.items.Count; i++)
        {
            //If there is an Item
            if(this.items[i] != null)
            {
                //Show the item sprite   
                children[i].GetComponentsInChildren<Image>()[1].sprite = this.items[i].inventoryImage;
            }
            else
            {
                //If not show the slot sprite (which is the default)
                children[i].GetComponentsInChildren<Image>()[1].sprite = this.inventorySlot.GetComponentsInChildren<Image>()[1].sprite;
            }

            if(i == this.activeItemIndex)
            {
                    children[i].GetComponentsInChildren<Image>()[0].sprite = selectedBackground[i % selectedBackground.Length];             
            }
            else
            {
                children[i].GetComponentsInChildren<Image>()[0].sprite = unselectedBackground[i % unselectedBackground.Length];
            }

        }
    }

    #region Getters & setters
    /** <summary>
        Returns the maximum amount to be held in the inventory.
        </summary>
    */
    public int GetMaxItems() {
        return this.maxItems;
    }

    /** <summary>
        Returns a reference to the active item, or `null` if there is none.
        </summary>
    */
    public Item GetActiveItem() {
        Item ret = null;
        if(this.activeItemIndex < this.items.Count) {
            ret = this.items[this.activeItemIndex];
        }
        return ret;
    }

    /** <summary>
        Changes the maximum amount of items. If reduced, items beyond the limit will be removed.
        If an item beyond the limit was active, the selection will be moved to the last item
        within the new limit.
        </summary>
    */
    public void SetMaxItems(int maxItems) {
        this.maxItems = maxItems;
        // Move selection to the last item, if current selection is beyond the limit
        if(this.activeItemIndex >= this.maxItems) {
            this.SetActiveItem(this.maxItems - 1);
        }
        // Remove any item beyond the limit
        if(this.items.Count > this.maxItems) {
            var overflowingItems = this.items.Count - this.maxItems;
            
            this.items.RemoveRange(this.items.Count - overflowingItems, overflowingItems);
        }else if(this.items.Count < this.maxItems)
        {
            while(this.items.Count < this.maxItems)
            {
                this.items.Add(null);
            }
        }
        Refresh();
    }
    /** <summary>
        Returns the index in the list of the active Item
        </summary>
    */
    public int GetActiveItemIndex()
    {
        return this.activeItemIndex;
    }


    /** <summary>
        Marks the item located at `index` position as active.
        </summary>
    */
    public void SetActiveItem(int index) {        
        this.activeItemIndex = Mathf.Clamp(index, 0, this.maxItems - 1);
        Refresh();
    }
    #endregion

    #region Add & remove items
    /** <summary>
        Attempts to add an item. The item is rejected if the inventory is full. Returns whether
        the attempt has been successful. Additionally triggers a HUD update.
        </summary>
    */
    public bool AddItem(Item item) {
        var success = false;
        if(this.items[activeItemIndex] == null)
        {
            this.items[activeItemIndex] = item;
            success = true;
        }
        this.Refresh();
        return success;
    }

    /** <summary>
        Removes the active item. Additionally triggers a HUD update.
        </summary>
    */
    public void RemoveActiveItem() {
        this.items[this.activeItemIndex] = null;
        this.Refresh();
    }

    /** <summary>
        Removes any item from the inventory and updates the HUD
        </summary>
    */
    public void ClearItems() {
        for(int i = 0; i < maxItems; i++)
        {
            this.items[i] = null;
        }
        this.activeItemIndex = 0;
        this.Refresh();
    }
    #endregion
}
