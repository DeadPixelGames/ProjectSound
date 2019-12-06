using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BubbleSpawner))]
public abstract class EnemyController : Entity, ISplashable {

    protected struct CapsuleData {
        private Vector3 point0;
        private Vector3 point1;
        private float height;
        private float radius;
        private Vector3 direction;
        private Transform transform;
        public CapsuleData(Transform transform, Vector3 center, float height, float radius, int direction) {
            this.radius = radius;
            this.height = height;
            this.transform = transform;
            this.direction = Vector3.zero;
            switch(direction) {
            case 0:
                this.direction = Vector3.right;
                break;
            case 1:
                this.direction = Vector3.up;
                break;
            case 2:
                this.direction = Vector3.forward;
                break;
            }
            var halfCapsule = (0.5f * height - radius) * this.direction;
            this.point0 = center - halfCapsule;
            this.point1 = center + halfCapsule;
        }

        public Vector3 GetPoint0() {
            return this.transform.TransformPoint(this.point0);
        }
        public Vector3 GetPoint1() {
            return this.transform.TransformPoint(this.point1);
        }
        public float GetHeight() {
            return this.height;
        }
        public float GetRadius() {
            return this.radius;
        }
        public Vector3 GetDirection() {
            return this.direction;
        }

    }

    public Transform shootTransform;

    public CapsuleCollider fieldOfView;

    public GameObject bulletPrefab;

    public float shootCooldown = 2f;

    public Item bubbleOnRegularDeath;

    public Item bubbleOnSplashInducedDeath;

    protected Animator animator;

    protected new Rigidbody rigidbody;

    protected bool moving;

    private Collider[] entitiesThisEnemyCanSee;

    private new CapsuleCollider collider;

    protected CapsuleData fieldOfViewData;

    protected CapsuleData colliderData;

    private BubbleSpawner spawner;

    private float playerSeenTimeout;

    private float playerShootTimeout;

    private float traverseLayerCounter;

    private float destRotation;

    private float destRotationCounter;

    private int previousLayer;

    private bool facingLeft;

    protected bool dead;

    #region Unity
    protected override void Awake() {
        base.Awake();
        this.animator = this.GetComponent<Animator>();
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.collider = this.GetComponent<CapsuleCollider>();
        this.spawner = this.GetComponent<BubbleSpawner>();
        this.fieldOfViewData = this.GetCapsuleColliderData(this.fieldOfView);
        this.colliderData = this.GetCapsuleColliderData(this.collider);
        this.entitiesThisEnemyCanSee = new Collider[8];
        this.destRotation = this.transform.rotation.eulerAngles.y;
    }

    protected override void Update() {
        base.Update();

        // If below death barrier, despawn
        if(this.transform.position.y < GameManager.instance.bottomlessPit.position.y) {
            GameObject.Destroy(this.gameObject);
        }

        // If dead, do not continue running checks
        if(this.dead) {
            return;
        }

        // Determine whether the player can be seen
        
        if(this.SeesPlayer()) {
            this.playerSeenTimeout = 1f;
        } else {
            this.playerSeenTimeout -= Time.deltaTime;
        }
        var playerSeen = (this.SeesPlayer() || this.playerSeenTimeout > 0) && !GameManager.instance.player.IsDead();
        this.animator.SetBool("SeesPlayer", playerSeen);

        // Move to the player's layer
        if(!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
            if(playerSeen && GameManager.instance.player.GetLayer() != this.layer && Random.value < 0.1f) {
                this.ChangeLayer(this.layer - GameManager.instance.player.GetLayer());
            }
        }

        // Shoot the player
        if(playerSeen && this.playerShootTimeout <= 0 && GameManager.instance.player.GetLayer() == this.layer) {
            this.animator.SetTrigger("Shoot");
            this.playerShootTimeout = this.shootCooldown;
        } else {
            this.playerShootTimeout -= Time.deltaTime;
        }

        // Traverse layers
        if(this.animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
            if(Mathf.Abs(this.transform.position.z - GameManager.instance.GetLayer(this.layer)) > float.Epsilon) {
                this.transform.position = Vector3.Slerp(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, GameManager.instance.GetLayer(this.layer)), this.traverseLayerCounter);
                this.traverseLayerCounter += 0.1f * Time.deltaTime;
            } else {
                this.snapToLayer = true;
                this.traverseLayerCounter = 0;
            }
        }

        // Rotate
        var euler = this.transform.rotation.eulerAngles;
        if(Mathf.Abs(this.transform.rotation.eulerAngles.y - this.destRotation) > float.Epsilon) {
            this.transform.rotation = Quaternion.Euler(Vector3.Slerp(euler, new Vector3(euler.x, destRotation, euler.z), this.destRotationCounter));
            this.destRotationCounter += 0.5f * Time.deltaTime;
        } else {
            this.destRotationCounter = 0;
        }

        // Die
        if(this.getHealth() <= 0) {
            this.animator.SetTrigger("Death");
            this.dead = true;

            this.spawner.SetItem(this.bubbleOnRegularDeath);
            this.spawner.Spawn();
        }
    }
    #endregion

    public abstract bool ShouldFlip();

    public void Shoot() {
        if(this.dead) {
            return;
        }

        var bullet = GameObject.Instantiate(this.bulletPrefab, this.shootTransform.position, Quaternion.identity).GetComponent<BulletEntity>();
        bullet.SetLayer(this.layer);
        bullet.SetDirection(facingLeft ? -1 : 1);
        
        this.spawner.Spawn();
    }

    public void ChangeLayer(int change) {
        RaycastHit hit;
        this.previousLayer = this.layer;

        if(this.dead) {
            return;
        }

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
            0x1 << 9, QueryTriggerInteraction.Ignore
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
        destRotation = (destRotation + 180) % 360;

        this.facingLeft = !this.facingLeft;
    }

    public bool IsFacingLeft() {
        return this.facingLeft;
    }

    public void SetFacingLeft(bool facingLeft) {
        if(this.facingLeft != facingLeft) {
            destRotation = (destRotation + 180) % 360;
        }

        this.facingLeft = facingLeft;
    }

    public bool IsMoving() {
        return this.moving;
    }

    public void SetMoving(bool value) {
        this.moving = value;
    }

    private CapsuleData GetCapsuleColliderData(CapsuleCollider data) {
        return new CapsuleData(this.transform, data.center, data.height, data.radius, data.direction);
    }

    public void Splash() {
        if(dead) {
            return;
        }

        this.animator.SetTrigger("Death");
        this.dead = true;

        this.spawner.SetItem(this.bubbleOnSplashInducedDeath);
        this.spawner.Spawn();
    }
}
