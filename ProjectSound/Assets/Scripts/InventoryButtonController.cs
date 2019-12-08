using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButtonController : MonoBehaviour
{
    [HideInInspector]
    public int id;
    private void Awake()
    {
        id = -1;
    }
    /* <summary>
     * Takes its id and selects the Inventory slot with the same id in the List
     * </summary>
¡    */
    public void changeActiveItemInInventory()
    {
        //if(GameManager.operatingInMobile)
            Inventory.instance.SetActiveItem(id);
    }
}
