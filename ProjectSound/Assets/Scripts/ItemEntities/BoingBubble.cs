using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoingBubble : ItemEntity
{

    private bool floating = true;

    [SerializeField]
    private Vector3 movementForce;

    private new Rigidbody rigidbody;

    #region Unity
    private void Awake() {
        this.rigidbody = this.GetComponent<Rigidbody>();
    }
    #endregion

    public override void Move(float move)
    {
        if (!this.floating)
        {
            rigidbody.AddForce(new Vector3(movementForce.x * move, movementForce.y, 0));
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
            rigidbody.isKinematic = false;
            rigidbody.velocity = Vector3.zero;
            Debug.Log(rigidbody.isKinematic);
        }
        
    }
}
