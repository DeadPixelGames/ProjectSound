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


    private int layer = 0;

    private bool grounded = false;
    private Rigidbody rigidBody;
    private float movementSmoothing = .05f;
    Vector3 velocity = Vector3.zero;
    private float groundRadius = .2f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;


    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;


    //Dirección de mirada del personaje
    public bool facingRight = false;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        

        if(OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }
    }

    public override void Move(float move)
    {
        Vector3 targetVelocity = new Vector3(move * walkingSpeed * 10f, rigidBody.velocity.y);
        rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref velocity, movementSmoothing);

        if(move > 0  && !facingRight)
        {
            flip();
        }else if(move < 0 && facingRight)
        {
            flip();
        }
    }

    public void changeLayer(float change)
    {
        
        if (change > 0)
        {
            layer = GameManager.instance.ClampLayer(layer-1);
            
        }
        else if(change < 0)
        {
            layer = GameManager.instance.ClampLayer(layer+1);
        }
        
    }

    /* Método para dar la vuelta al sprite del personaje */
    public void flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;

        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /* Método para usar una onomatopeya */
    public void useBubble() { }


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
    }

    public new void jump()
    {
        if (grounded)
        {
            rigidBody.AddForce(new Vector2(0f, jumpSpeed));
        }
    }
}
