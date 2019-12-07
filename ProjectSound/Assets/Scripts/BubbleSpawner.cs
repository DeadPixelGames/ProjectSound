using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>
    Component that spawns onomatopoeia bubbles associated to an item.
    </summary>
*/
[RequireComponent(typeof(AudioSource))]
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

    public int defaultLayer;

    private ItemEntity lastSpawnedItem;

    private Entity entity;

    private new AudioSource audio;

    private void Awake() {
        this.entity = this.GetComponent<Entity>();
        this.audio = this.GetComponent<AudioSource>(); 
    }

    /** <summary>
        Instantiates the bubble associated with the `item` property at the position of the
        `spawnTransform`, or at the position of this GameObject if none is provided.
        </summary>
    */
    public void Spawn() {
        if(!this.CanSpawnItem()) {
            this.PlayBubbleSound();
            return;
        }

        var spawnTransform = this.spawnTransform != null ? this.spawnTransform : this.transform;
        this.lastSpawnedItem = GameObject.Instantiate(this.item.itemEntityPrefab, spawnTransform.position, Quaternion.identity).GetComponent<ItemEntity>();
        if(this.entity != null) {
            this.lastSpawnedItem.SetLayer(this.entity.GetLayer());
        } else {
            this.lastSpawnedItem.SetLayer(this.defaultLayer);
        }
    }

    public void SetItem(Item item) {
        this.item = item;
        this.lastSpawnedItem = null;
    }

    private bool CanSpawnItem() {
        return this.lastSpawnedItem == null;
    }

    private void PlayBubbleSound() {
        if(this.audio != null) {
            this.audio.clip = this.item.itemEntityPrefab.GetComponent<ItemEntity>().GetSoundFromPrefab();
            this.audio.Play();
        }
    }
}
