using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>
    Component that spawns onomatopoeia bubbles associated to an item.
    </summary>
*/
public class BubbleSpawner : MonoBehaviour {

    /** <summary>
        The item from which to take the bubble prefab.
        </summary>
    */
    public Item item;

    /** <summary>
        Optional transform that indicates where the bubbles should spawn, if different
        from the position of the associated GameObject.
        </summary>
    */
    public Transform spawnTransform;

    private ItemEntity lastSpawnedItem;

    /** <summary>
        Instantiates the bubble associated with the `item` property at the position of the
        `spawnTransform`, or at the position of this GameObject if none is provided.
        </summary>
    */
    public void Spawn() {
        if(!this.CanSpawnItem()) {
            return;
        }

        var spawnTransform = this.spawnTransform != null ? this.spawnTransform : this.transform;
        this.lastSpawnedItem = GameObject.Instantiate(item.itemEntityPrefab, spawnTransform.position, Quaternion.identity).GetComponent<ItemEntity>();
    }

    public void SetItem(Item item) {
        this.item = item;
        this.lastSpawnedItem = null;
    }

    private bool CanSpawnItem() {
        return this.lastSpawnedItem == null;
    }
}
