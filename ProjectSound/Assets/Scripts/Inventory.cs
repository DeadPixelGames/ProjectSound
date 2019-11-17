using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject inventoryUI;

   [SerializeField]
    private int maxItems;

    private int activeItemIndex;

    private List<Item> items;

    #region Unity
    void Awake() {
        Inventory.InitSingleton(this);
        this.activeItemIndex = 0;
        this.items = new List<Item>();
    }
    #endregion

    private void Refresh() {
        //TODO Actualización del HUD
        // La interfaz gráfica del inventario se podría ocultar, o al menos
        // ocultar la marca de selección, si this.items.Count == 0
    }

    #region Getters & setters
    public int GetMaxItems() {
        return this.maxItems;
    }

    public Item GetActiveItem() {
        Item ret = null;
        if(this.activeItemIndex < this.items.Count) {
            ret = this.items[this.activeItemIndex];
        }
        return ret;
    }

    public void SetMaxItems(int maxItems) {
        this.maxItems = maxItems;
        if(this.activeItemIndex >= this.maxItems) {
            this.SetActiveItem(this.maxItems - 1);
        }
    }

    public void SetActiveItem(int index) {
        this.activeItemIndex = Mathf.Clamp(index, 0, this.maxItems - 1);
    }
    #endregion

    #region Add & remove items
    public bool AddItem(Item item) {
        var success = false;
        if(this.items.Count < this.maxItems) {
            this.items.Add(item);
            this.activeItemIndex = this.items.Count;
        } else {
            var overflowingItems = this.items.Count - this.maxItems;
            this.items.RemoveRange(this.items.Count - overflowingItems, overflowingItems);
        }
        this.Refresh();
        return success;
    }

    public void RemoveActiveItem() {
        this.items.RemoveAt(this.activeItemIndex);
        if(this.activeItemIndex >= this.items.Count) {
            if(this.items.Count != 0) {
                this.activeItemIndex = this.items.Count - 1;
            } else {
                this.activeItemIndex = 0;
            }
        }
        this.Refresh();
    }

    public void ClearItems() {
        this.items.Clear();
        this.activeItemIndex = 0;
        this.Refresh();
    }
    #endregion
}
