using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBubble : ItemEntity {

    public float damage = 10f;

    public float effectRadius = 2f;

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
            this.Explode();
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this.effectRadius);
    }
    #endregion

    public override void Move(float move) {
        if(!this.floating) {
            rigidbody.AddForce(new Vector3(movementForce.x * move, movementForce.y, 0));
        }
    }

    public override void Use(int direction, Vector3 position) {
        this.transform.position = position;
        this.floating = false;
        this.Move(direction);
    }

    private void Explode() {
        var colliders = Physics.OverlapSphere(this.transform.position, this.effectRadius);
        foreach(Collider collider in colliders) {
            var explodableComponents = collider.gameObject.GetComponents<IExplodable>();
            foreach(IExplodable explodable in explodableComponents) {
                explodable.Explode();
            }
            var entity = collider.gameObject.GetComponent<Entity>();
            if(entity != null) {
                entity.addHealth(this.damage);
            }
        }
        GameObject.Destroy(this.gameObject);
    }
}
