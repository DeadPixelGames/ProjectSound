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

    private int layer = 0;

    private bool grounded = false;
    private Rigidbody rigidBody;
    private float movementSmoothing = .05f;
    Vector3 velocity = Vector3.zero;
    private float groundRadius = .2f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsBouncy;


    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;
    public UnityEvent OnBouncyEvent;


    //Dirección de mirada del personaje
    public bool facingLeft = false;

    private void Awake()
    {
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
        Vector3 targetVelocity = new Vector3(move * walkingSpeed * 10f, rigidBody.velocity.y);
        rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref velocity, movementSmoothing);

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
            
            if(Physics.Raycast(new Ray(transform.position, new Vector3(0f, 0f, 1f)), out hit))
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

            if (Physics.Raycast(new Ray(transform.position, new Vector3(0f, 0f, -1f)), out hit))
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
        GameObject bubble = GameObject.Instantiate(GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>().GetActiveItem().itemEntityPrefab);
        Vector3 pos = this.transform.position;
        if (this.facingLeft)
        {
            pos.x -= 1f;
            pos.y += 1f;
            bubble.GetComponent<ItemEntity>().Use(-1, pos);
        }
        else
        {
            pos.x += 1f;
            pos.y += 1f;
            bubble.GetComponent<ItemEntity>().Use(1, pos);
        }

        GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>().RemoveActiveItem();
    }


    private void FixedUpdate()
    {
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
            rigidBody.AddForce(new Vector2(0f, jumpSpeed));
        }
    }


    public void bounce()
    {

        if (Input.GetButton("Jump"))
        {
            Debug.Log("UwU");
            rigidBody.AddForce(new Vector2(0f, jumpSpeed * 0.025f), ForceMode.Impulse);
        }
    }


}
