﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : Entity {

    private struct CapsuleData {
        private Vector3 point0;
        private Vector3 point1;
        private float radius;
        private Transform transform;
        public CapsuleData(Transform transform, Vector3 point0, Vector3 point1, float radius) {
            this.point0 = point0;
            this.point1 = point1;
            this.radius = radius;
            this.transform = transform;
        }

        public Vector3 GetPoint0() {
            return this.transform.TransformPoint(this.point0);
        }
        public Vector3 GetPoint1() {
            return this.transform.TransformPoint(this.point1);
        }
        public float GetRadius() {
            return this.radius;
        }

    }

    public Transform shootTransform;

    public CapsuleCollider fieldOfView;

    public GameObject bulletPrefab;

    public float shootCooldown = 2f;

    protected Animator animator;

    private Collider[] entitiesThisEnemyCanSee;

    private CapsuleData fieldOfViewData;

    private float playerSeenTimeout;

    private float playerShootTimeout;

    private int previousLayer;

    private bool facingLeft;

    #region Unity
    private new void Awake() {
        base.Awake();
        this.fieldOfViewData = this.GetFieldOfViewData();
        this.animator = this.GetComponent<Animator>();
        this.entitiesThisEnemyCanSee = new Collider[8];
    }

    private new void Update() {
        base.Update();

        var playerSeen = this.SeesPlayer();
        if(playerSeen) {
            this.playerSeenTimeout = 1f;
        } else {
            this.playerSeenTimeout -= Time.deltaTime;
        }
        this.animator.SetBool("SeesPlayer", playerSeen || this.playerSeenTimeout > 0);
        if((playerSeen || this.playerSeenTimeout > 0) && GameManager.instance.player.GetLayer() != this.layer) {
            this.ChangeLayer(this.layer - GameManager.instance.player.GetLayer());
        }
        if((playerSeen || this.playerSeenTimeout > 0) && this.playerShootTimeout <= 0) {
            this.animator.SetTrigger("Shoot");
            this.playerShootTimeout = this.shootCooldown;
        } else {
            this.playerShootTimeout -= Time.deltaTime;
        }
        if(Mathf.Abs(this.transform.position.z - GameManager.instance.GetLayer(this.layer)) > float.Epsilon) {
            this.transform.position = Vector3.Slerp(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, GameManager.instance.GetLayer(this.layer)), this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        } else {
            this.snapToLayer = true;
        }
        if(this.getHealth() <= 0) {
            this.animator.SetTrigger("Death");
        }
    }
    #endregion

    public abstract bool ShouldTurnAround();

    public void Shoot() {
        var bullet = GameObject.Instantiate(this.bulletPrefab, this.shootTransform.position, Quaternion.identity);
        bullet.GetComponent<BulletEntity>().SetDirection(facingLeft ? -1 : 1);
    }

    public void ChangeLayer(int change) {
        RaycastHit hit;
        this.previousLayer = this.layer;

        if(change > 0) {
            if(Physics.Raycast(this.transform.position, Vector3.forward, out hit, Mathf.Infinity, 0x7FFFFFFF, QueryTriggerInteraction.Ignore)) {
                if(hit.point.z > GameManager.instance.GetLayer(layer - 1)) {
                    this.layer = GameManager.instance.ClampLayer(layer - 1);
                }
            } else {
                this.layer = GameManager.instance.ClampLayer(layer - 1);
            }
        } else {
            if(Physics.Raycast(this.transform.position, Vector3.back, out hit, Mathf.Infinity, 0x7FFFFFFF, QueryTriggerInteraction.Ignore)) {
                if(hit.point.z < GameManager.instance.GetLayer(layer + 1)) {
                    this.layer = GameManager.instance.ClampLayer(this.layer + 1);
                } else {
                    this.layer = GameManager.instance.ClampLayer(layer + 1);
                }
            }
        }

        this.snapToLayer = false;
        this.animator.SetTrigger("Jump");
    }

    public bool SeesPlayer() {
        var seenCollidersAmount = Physics.OverlapCapsuleNonAlloc(
            this.fieldOfViewData.GetPoint0(),
            this.fieldOfViewData.GetPoint1(),
            this.fieldOfViewData.GetRadius(),
            this.entitiesThisEnemyCanSee,
            0x7FFFFFFF, QueryTriggerInteraction.Ignore
        );

        var ret = false;
        for(int i = 0; i < seenCollidersAmount; i++) {
            if(this.entitiesThisEnemyCanSee[i].gameObject.GetComponent<Player>() != null) {
                ret = true;
            }
        }
        return ret;
    }

    public void Flip() {
        var eulerAngles = this.transform.rotation.eulerAngles;
        eulerAngles.y = (eulerAngles.y + 180) % 360;
        this.transform.rotation = Quaternion.Euler(eulerAngles);

        this.facingLeft = !this.facingLeft;
    }

    public void SetFacingLeft(bool facingLeft) {
        var eulerAngles = this.transform.rotation.eulerAngles;
        if(this.facingLeft != facingLeft) {
            eulerAngles.y = (eulerAngles.y + 180) % 360;
        }
        this.transform.rotation = Quaternion.Euler(eulerAngles);

        this.facingLeft = facingLeft;
    }

    private CapsuleData GetFieldOfViewData() {
        Vector3 direction = Vector3.zero;
        switch(this.fieldOfView.direction) {
            case 0:
                direction = Vector3.right;
                break;
            case 1:
                direction = Vector3.up;
                break;
            case 2:
                direction = Vector3.forward;
                break;
        }
        var halfCapsule = (0.5f * this.fieldOfView.height - this.fieldOfView.radius) * direction;
        return new CapsuleData(
            this.transform,
            this.fieldOfView.center - halfCapsule,
            this.fieldOfView.center + halfCapsule,
            this.fieldOfView.radius
        );
    }
}
