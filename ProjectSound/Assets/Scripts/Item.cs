using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** <summary>
    Class that represents an item as part of the inventory. This is different from an ItemEntity
    object in that it is not a GameObject, and therefore does not need to be present in the world
    at all times.
    </summary>
    */
[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject {
    /** <summary>
        Image to use in the inventory to represent this item.
        </summary>
    */
    public Sprite inventoryImage;

    /** <summary>
        Prefab to instantiate whenever this item is brought to the game world.
        </summary>
    */
    public GameObject itemEntityPrefab;
}
