using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /** <summary>
        Reference to the HUD area displaying the inventory, so it can be updated.
        </summary>
    */
    public GameObject inventoryUI;

    /** <summary>
        Maximum amount of items that can be held at any given time.
        </summary>
    */
   [SerializeField]
    private int maxItems;

    /** <summary>
        Index of the active item in the list, or `null` if no item is selected.
        </summary>
    */
    private int? activeItemIndex;

    /** <summary>
        List of items held in the inventory.
        </summary>
    */
    private List<Item> items;

    #region Unity
    void Awake() {
        Inventory.InitSingleton(this);
        // No item selected at first
        this.activeItemIndex = null;
        this.items = new List<Item>();
    }
    #endregion

    /** <summary>
        Updates the HUD so it displays the current state of the inventory.
        </summary>
    */
    private void Refresh() {
        //TODO Actualización del HUD
        // La interfaz gráfica del inventario se podría ocultar si this.items.Count == 0
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
        if(this.activeItemIndex.HasValue && this.activeItemIndex < this.items.Count) {
            ret = this.items[this.activeItemIndex.Value];
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
        }
    }

    /** <summary>
        Marks the item located at `index` position as active, or deactivates the currently
        active item if `index` is `null`.
        </summary>
    */
    public void SetActiveItem(int? index) {
        if(index.HasValue) {
            this.activeItemIndex = Mathf.Clamp(index.Value, 0, this.maxItems - 1);
        } else {
            this.activeItemIndex = null;
        }
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
        if(this.items.Count < this.maxItems) {
            this.items.Add(item);
            this.activeItemIndex = this.items.Count;
        }
        this.Refresh();
        return success;
    }

    /** <summary>
        Removes the active item. The selection is passed to a different item. If no item is
        active, or the inventory is empty, this method has no effect. Additionally triggers
        a HUD update.
        </summary>
    */
    public void RemoveActiveItem() {
        if(!this.activeItemIndex.HasValue) {
            return;
        }
        this.items.RemoveAt(this.activeItemIndex.Value);
        if(this.activeItemIndex >= this.items.Count) {
            if(this.items.Count != 0) {
                this.activeItemIndex = this.items.Count - 1;
            } else {
                this.activeItemIndex = null;
            }
        }
        this.Refresh();
    }

    /** <summary>
        Removes any item from the inventory and updates the HUD
        </summary>
    */
    public void ClearItems() {
        this.items.Clear();
        this.activeItemIndex = null;
        this.Refresh();
    }
    #endregion
}
