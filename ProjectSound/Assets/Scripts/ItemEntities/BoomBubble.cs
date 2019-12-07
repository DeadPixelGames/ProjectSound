using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBubble : ItemEntity {

    public float damage = 10f;

    public float destroyRadius = 2f;

    public float pushRadius = 3f;

    public float pushFactor = 1f;

    public float pushDecay = 1f;

    [SerializeField]
    private Vector3 movementForce;

    private float cooldown = 0.05f;

    #region Unity
    protected override void Awake() {
        base.Awake();
    }

    private new void FixedUpdate() {
        base.FixedUpdate();
        if(!this.floating) {
            this.cooldown -= Time.deltaTime;
        }
    }
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject == GameManager.instance.player.gameObject) {
            return;
        }
        if(!this.floating && this.cooldown < 0) {
            this.Push();
            this.Explode();
            this.PlaySound();
        }
    }

    private void OnCollisionStay(Collision other) {
        this.OnCollisionEnter(other);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this.destroyRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, this.pushRadius);
    }
    #endregion

    public override void Move(float move) {
        if(!this.floating) {
            this.rb.AddForce(new Vector3(100f * movementForce.x * move, movementForce.y, 0));
        }
    }

    public override void Use(int direction, Vector3 position) {
        this.transform.position = position;
        this.floating = false;
        this.Move(direction);
    }

    private void Explode() {
        var colliders = Physics.OverlapSphere(this.transform.position, this.destroyRadius);
        foreach(Collider collider in colliders) {
            var explodableComponents = collider.gameObject.GetComponents<IExplodable>();
            foreach(IExplodable explodable in explodableComponents) {
                explodable.Explode();
            }
            var entity = collider.gameObject.GetComponent<Entity>();
            if(entity != null) {
                entity.addHealth(-this.damage);
            }
        }
        GameObject.Destroy(this.gameObject);
    }

    private void Push() {
        var colliders = Physics.OverlapSphere(this.transform.position, this.pushRadius);
        foreach(Collider collider in colliders) {
            var rigidbodies = collider.gameObject.GetComponents<Rigidbody>();
            foreach(Rigidbody rigidbody in rigidbodies) {
                var direction = (- this.transform.position + rigidbody.transform.position).normalized;
                if(Vector3.Distance(this.transform.position, rigidbody.transform.position) > this.pushRadius) {
                    continue;
                }
                var force = this.pushRadius * direction - pushDecay * (- this.transform.position + rigidbody.transform.position);
                rigidbody.AddForce(force * pushFactor, ForceMode.Impulse);
            }
        }
    }
}
