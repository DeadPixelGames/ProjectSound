using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : EnemyController
{

    public float speed = 5f;

    public float horizontalLimit = 10f;

    private Vector3 spawnPosition;
    
    private CapsuleCollider capsuleCollider;

    protected override void Awake() {
        base.Awake();
        this.spawnPosition = this.transform.position;
        this.capsuleCollider = this.GetComponent<CapsuleCollider>();
    }

    public override void Move(float move) {
        if(this.dead) {
            return;
        }

        if(this.moving) {
            this.rigidbody.MovePosition(this.transform.position + 0.1f * this.speed * Time.deltaTime * (this.IsFacingLeft() ? -1 : 1) * Vector3.right);
        }

        var seesPlayer = this.SeesPlayer() && !GameManager.instance.player.IsDead();
        if(!seesPlayer && this.transform.position.y < this.spawnPosition.y
        || seesPlayer && this.transform.position.y < GameManager.instance.player.transform.position.y) {
            this.rigidbody.AddForce(-0.05f * Physics.gravity, ForceMode.Impulse);
        } else if(!seesPlayer && this.transform.position.y > this.spawnPosition.y
        || seesPlayer && this.transform.position.y > GameManager.instance.player.transform.position.y) {
            this.rigidbody.AddForce(0.05f * Physics.gravity, ForceMode.Impulse);
        }
    }

    public override bool ShouldFlip() {
        if(this.dead) {
            return false;
        }

        var ret = false;
        RaycastHit hit;

        var direction = this.IsFacingLeft() ? -1 : 1;

        var hitWall = this.rigidbody.SweepTest(direction * Vector3.right, out hit, this.speed * Time.deltaTime, QueryTriggerInteraction.Ignore);
        var strayedTooFar = Mathf.Abs(this.transform.position.x - this.spawnPosition.x) >= this.horizontalLimit;

        if(hitWall || strayedTooFar) {
            ret = true;
        }
        return ret;
    }
}
