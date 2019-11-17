using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    public Item item;

    public Transform spawnTransform;

    public void Spawn() {
        var spawnTransform = this.spawnTransform != null ? this.spawnTransform : this.transform;

        GameObject.Instantiate(item.itemEntityPrefab, spawnTransform.position, Quaternion.identity);
    }
}
