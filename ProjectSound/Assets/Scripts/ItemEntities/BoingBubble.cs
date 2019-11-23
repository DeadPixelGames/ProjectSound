using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoingBubble : ItemEntity
{

    private bool floating = true;

    [SerializeField]
    private Vector3 movementForce;

    public override void Move(float move)
    {
        if (!this.floating)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(movementForce.x * move, movementForce.y, 0));
        }
    }

    public override void Use(int direction, Vector3 position)
    {
        this.transform.position = position;
        this.floating = false;
        Move(direction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject != GameObject.FindGameObjectWithTag("Player"))
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log(GetComponent<Rigidbody>().isKinematic);
        }
        
    }
}
