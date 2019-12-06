using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandEnemyController : EnemyController {

    public float speed = 5f;

    private CapsuleCollider capsuleCollider;

    protected override void Awake() {
        base.Awake();
        this.capsuleCollider = this.GetComponent<CapsuleCollider>();
    }

    public override void Move(float move) {
        if(this.dead) {
            return;
        }

        if(this.moving) {
            this.rigidbody.MovePosition(this.transform.position + 0.1f * this.speed * Time.deltaTime * (this.IsFacingLeft() ? -1 : 1) * Vector3.right);
        }
    }

    public override bool ShouldFlip() {
        var ret = false;

        if(this.dead) {
            return false;
        }

        RaycastHit hit;

        var direction = this.IsFacingLeft() ? -1 : 1;
        var offset = this.speed * Time.deltaTime * direction * Vector3.right;

        var reachedPit = !Physics.Raycast(this.capsuleCollider.bounds.center, Vector3.down, this.capsuleCollider.bounds.size.y * 0.5f, 0x7FFFFFFF, QueryTriggerInteraction.Ignore);
        var hitWall = this.rigidbody.SweepTest(direction * Vector3.right, out hit, this.speed * Time.deltaTime, QueryTriggerInteraction.Ignore);
        // IMPORTANT! In order for hitWall to work, the field of view collider must have its own rigidbody. Otherwise, SweepTest will use it
        // as a regular, non-trigger collider, which will result in many false positives.
        
        if(reachedPit || hitWall) {
            ret = true;
        }
        return ret;
    }
}
