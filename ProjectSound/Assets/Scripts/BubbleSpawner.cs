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

    /** <summary>
        Instantiates the bubble associated with the `item` property at the position of the
        `spawnTransform`, or at the position of this GameObject if none is provided.
        </summary>
    */
    public void Spawn() {
        var spawnTransform = this.spawnTransform != null ? this.spawnTransform : this.transform;

        GameObject.Instantiate(item.itemEntityPrefab, spawnTransform.position, Quaternion.identity);
    }
}
