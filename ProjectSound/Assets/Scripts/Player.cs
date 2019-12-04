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
    private Rigidbody rigidBody;
    private float movementSmoothing = .05f;
    Vector3 velocity = Vector3.zero;
    private float groundRadius = .2f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsBouncy;
    [SerializeField] private Transform throwItem;


    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;
    public UnityEvent OnBouncyEvent;


    //Dirección de mirada del personaje
    public bool facingLeft = false;

    protected override void Awake() {
        base.Awake();

        rigidBody = GetComponent<Rigidbody>();
        

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

        rigidBody.MovePosition(this.transform.position + move * walkingSpeed * Vector3.right * 0.1f);

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
        if((previousLayer == LAYOUT_LAYER && layer != LAYOUT_LAYER) || (previousLayer != LAYOUT_LAYER && layer == LAYOUT_LAYER))
        {
            gameObject.GetComponent<Animator>().SetTrigger("ToggleClimb");
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
        var activeItem = Inventory.instance.GetActiveItem();
        if(activeItem == null) {
            return;
        }

        var bubble = GameObject.Instantiate(activeItem.itemEntityPrefab).GetComponent<ItemEntity>();
        bubble.Use(this.facingLeft ? -1 : 1, this.throwItem.position);
        bubble.SetLayer(this.layer);
        Inventory.instance.RemoveActiveItem();
    }


    private new void FixedUpdate() {

        #region Set Z layer
        Vector3 pos = transform.position;
        pos.z = GameManager.instance.GetLayer(layer);
        transform.position = pos;
        #endregion

        bool wasGrounded = grounded;

        grounded = false;

        Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundRadius, whatIsGround);

        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if(!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }
        }

        Collider[] bouncyColliders = Physics.OverlapSphere(groundCheck.position, groundRadius, whatIsBouncy);
        for(int i = 0; i < bouncyColliders.Length; i++)
        {
            if(bouncyColliders[i].gameObject != gameObject)
            {
                   OnBouncyEvent.Invoke();
                
            }
        }
    }

    public new void jump()
    {
        if (grounded)
        {
            rigidBody.AddForce(jumpSpeed * Vector3.up);
        }
    }


    public void bounce()
    {

        if (Input.GetButton("Jump"))
        {
            rigidBody.AddForce(jumpSpeed * 0.025f * Vector3.up, ForceMode.Impulse);
        }
    }


}
