using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashBubble : ItemEntity {
    
    private bool floating = true;

    [SerializeField]
    private Vector3 movementForce;

    private new Rigidbody rigidbody;

    private float cooldown = 0.1f;

    #region Unity
    private new void Awake() {
        base.Awake();
        this.rigidbody = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if(!this.floating) {
            this.cooldown -= Time.deltaTime;
        }
    }
    
    private void OnCollisionEnter(Collision other) {
        if(!this.floating && this.cooldown < 0) {
            this.Splash(other);
        }
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

    private void Splash(Collision other) {
        var splashableComponents = other.gameObject.GetComponents<ISplashable>();
        foreach(ISplashable splashable in splashableComponents) {
            splashable.Splash();
        }
        GameObject.Destroy(this.gameObject);
    }
}
