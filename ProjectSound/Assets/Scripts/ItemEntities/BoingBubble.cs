using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoingBubble : ItemEntity
{

    [SerializeField]
    private Vector3 movementForce;

    #region Unity
    protected override void Awake() {
        base.Awake();
    }
    #endregion

    public override void Move(float move) {
        if (!this.floating) {
            this.rb.AddForce(new Vector3(100f * movementForce.x * move, movementForce.y, 0));
        }
    }

    public override void Use(int direction, Vector3 position)
    {
        this.transform.position = position;
        this.floating = false;
        this.Move(direction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject != GameManager.instance.player) {
            this.rb.isKinematic = false;
            this.rb.velocity = Vector3.zero;
            this.PlaySound();
        }
        
    }
}
