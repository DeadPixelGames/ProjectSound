using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEntity : Entity
{

    public float speed;

    public float lifespan;

    public float damage;
    
    private int direction;

    private float currentLifespan;

    private new Rigidbody rigidbody;

    #region Unity
    protected override void Awake() {
        base.Awake();
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.currentLifespan = this.lifespan;
    }

    protected override void Update() {
        base.Update();
        if(this.currentLifespan <= 0) {
            GameObject.Destroy(this.gameObject);
        } else {
            this.currentLifespan -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<Player>();
        if(player != null) {
            player.addHealth(-this.damage);
            GameObject.Destroy(this.gameObject);
        }
    }
    #endregion

    public override void Move(float move) {
        this.rigidbody.MovePosition(this.transform.position + (this.speed * this.direction * Time.deltaTime) * Vector3.right);
    }

    public void SetDirection(int direction) {
        this.direction = direction;
    }
}
