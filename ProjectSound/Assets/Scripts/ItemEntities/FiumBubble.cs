using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiumBubble : ItemEntity {

    public float damage = 10f;

    public float impulse; 

    private bool floating = true;

    [SerializeField]
    private Vector3 movementForce;

    private new Rigidbody rigidbody;

    private float cooldown = 0.1f;

    #region Unity
    private void Awake() {
        this.rigidbody = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if(!this.floating) {
            this.cooldown -= Time.deltaTime;
        }
        
    }
    
    private void OnCollisionEnter(Collision other) {
        if(!this.floating && this.cooldown < 0) {
            this.Damage(other);
        }
    }
    #endregion

    public override void Move(float move) {
        if(!this.floating) {
            this.rigidbody.AddForce(move * new Vector3(1, 0, 0), ForceMode.Impulse);
        }
    }

    public override void Use(int direction, Vector3 position) {
        this.transform.position = position;
        this.floating = false;
        this.Move(direction * impulse);
    }

    private void Damage(Collision other) {
        var entity = other.gameObject.GetComponent<Entity>();
        if(entity != null) {
            //entity.addHealth(this.damage);
        }
        GameObject.Destroy(this.gameObject);
    }
}
