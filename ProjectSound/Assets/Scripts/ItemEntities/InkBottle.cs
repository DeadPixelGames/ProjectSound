using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBottle : ItemEntity {

    public float heal = 1f;

    public override void Use(int direction, Vector3 position) {
        GameManager.instance.player.addHealth(this.heal);
        GameObject.Destroy(this.gameObject);
    }
}
