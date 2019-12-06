using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/** <summary>
    Component controlling the entity representing the player character.
    </summary>
*/
public class Player : Entity
{

    /** <summary>
        Event that triggers when the player dies.
        </summary>
    */
    public event System.Action onPlayerDead;

    private const int LAYOUT_LAYER = 3;
    private int previousLayer = 0;
    private bool grounded = false;

    private bool dead = false;
    private float previousHealth;
    private Rigidbody rigidBody;
    private float movementSmoothing = .05f;
    Vector3 velocity = Vector3.zero;
    private float groundRadius = .2f;
    private Collider[] overlappedColliders;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsBouncy;
    [SerializeField] private Transform throwItem;

    [SerializeField] private float movementForce;
    [SerializeField] private float maxVelocity;
    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;
    public UnityEvent OnBouncyEvent;


    private Animator animator;


    //Dirección de mirada del personaje
    public bool facingLeft = false;

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();

        rigidBody = GetComponent<Rigidbody>();
        
        overlappedColliders = new Collider[8];

        if(OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }
        if(OnBouncyEvent == null)
        {
            OnBouncyEvent = new UnityEvent();
        }
    }

    public override void Move(float move)
    {

        if(this.dead) {
            return;
        }
        
        //rigidBody.MovePosition(this.transform.position + move * walkingSpeed * Vector3.right);
        //rigidBody.AddForce(movementForce * move * walkingSpeed * Vector3.right);

        //if(rigidBody.velocity.magnitude > maxVelocity)
        //{
        //    rigidBody.velocity = new Vector2(maxVelocity,0);
        //}
        transform.Translate(new Vector3(move, 0, 0) * walkingSpeed);


        if(move < 0  && !facingLeft)
        {
            flip();
        }else if(move > 0 && facingLeft)
        {
            flip();
        }
    }

    public void changeLayer(float change)
    {

        if(this.dead) {
            return;
        }

        RaycastHit hit;

        previousLayer = layer;
        if (change > 0)
        {
            
            if(Physics.Raycast(transform.position, new Vector3(0f, 0f, 1f), out hit, Mathf.Infinity, 0x7FFFFFFF, QueryTriggerInteraction.Ignore))
            {
                if(hit.point.z > GameManager.instance.GetLayer(layer - 1)){
                    layer = GameManager.instance.ClampLayer(layer - 1);
                }
            }
            else
            {
                layer = GameManager.instance.ClampLayer(layer - 1);
            }
            
        }
        else if(change < 0)
        {

            if (Physics.Raycast(transform.position, new Vector3(0f, 0f, -1f), out hit, Mathf.Infinity, 0x7FFFFFFF, QueryTriggerInteraction.Ignore))
            {
                if (hit.point.z < GameManager.instance.GetLayer(layer + 1))
                {
                    layer = GameManager.instance.ClampLayer(layer+1);
                }
            }
            else
            {
                layer = GameManager.instance.ClampLayer(layer + 1);
            }
            
        }
        if((previousLayer == LAYOUT_LAYER && layer != LAYOUT_LAYER) )
        {
            animator.SetBool("Layout", false);
        }
        if ((previousLayer != LAYOUT_LAYER && layer == LAYOUT_LAYER))
        {
            animator.SetTrigger("ToggleLayout");
            animator.SetBool("Layout", true);
        }

        
    }

    /* Método para dar la vuelta al sprite del personaje */
    public void flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;

        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /* Método para usar una onomatopeya */
    public void useBubble() {

        if(this.dead) { 
            return;
        }

        var activeItem = Inventory.instance.GetActiveItem();
        if(activeItem == null) {
            return;
        }

        var bubble = GameObject.Instantiate(activeItem.itemEntityPrefab).GetComponent<ItemEntity>();
        bubble.Use(this.facingLeft ? -1 : 1, this.throwItem.position);
        bubble.SetLayer(this.layer);
        Inventory.instance.RemoveActiveItem();
    }


    protected override void FixedUpdate() {
        base.FixedUpdate();

        if(this.dead) {
            return;
        }

        #region Set Z layer
        Vector3 pos = transform.position;
        pos.z = GameManager.instance.GetLayer(layer);
        transform.position = pos;
        #endregion

        bool wasGrounded = grounded;

        grounded = false;

        Physics.OverlapSphereNonAlloc(groundCheck.position, groundRadius, overlappedColliders, whatIsGround);

        for(int i = 0; i < overlappedColliders.Length; i++)
        {
            if(overlappedColliders[i] == null) {
                 break;
            }

            if(overlappedColliders[i].gameObject != this.gameObject)
            {
                grounded = true;
                if(!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }

            overlappedColliders[i] = null;
        }

        Physics.OverlapSphereNonAlloc(groundCheck.position, groundRadius, overlappedColliders, whatIsBouncy);
        
        for(int i = 0; i < overlappedColliders.Length; i++)
        {
            if(overlappedColliders[i] == null) {
                break;
            }

            if(overlappedColliders[i].gameObject != this.gameObject)
            {
                OnBouncyEvent.Invoke();
                break;
            }

            overlappedColliders[i] = null;
        }
        
        animator.SetFloat("Life", this.getHealth());

        if(this.getHealth() <= 0) {
            this.dead = true;
            this.animator.SetTrigger("Death");
            this.onPlayerDead.Invoke();
        }
    }

    public new void jump()
    {
        if(this.dead) {
            return;
        }

        if (grounded)
        {
            
            rigidBody.AddForce(jumpSpeed * Vector3.up);
            animator.SetBool("Grounded", false);
        
        }
    }


    public void bounce()
    {
        if(this.dead) {
            return;
        }

        if (Input.GetButton("Jump"))
        {
            rigidBody.AddForce(jumpSpeed * 0.025f * Vector3.up, ForceMode.Impulse);
        }
    }

    public bool IsDead() {
        return this.dead;
    }
}
