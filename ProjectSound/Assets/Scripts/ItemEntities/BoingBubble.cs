using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoingBubble : ItemEntity
{

    [SerializeField]
    private Vector3 movementForce;

    private new Rigidbody rigidbody;

    #region Unity
    protected override void Awake() {
        base.Awake();
        this.rigidbody = this.GetComponent<Rigidbody>();
    }
    #endregion

    public override void Move(float move) {
        if (!this.floating) {
            this.rigidbody.AddForce(new Vector3(movementForce.x * move, movementForce.y, 0));
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
        if(collision.gameObject != GameObject.FindGameObjectWithTag("Player"))
        {
            this.rigidbody.isKinematic = false;
            this.rigidbody.velocity = Vector3.zero;
        }
        
    }
}
