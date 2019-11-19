using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Entity
{
    public float layerWidth;
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
    bool facingRight = false;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        

        if(OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }
    }

    public new void move(float move)
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
        Vector3 pos = transform.position;
        if (change > 0)
        {
            pos.z += layerWidth;
        }
        else if(change < 0)
        {
            pos.z -= layerWidth;
        }
        transform.position = pos;
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
    public void useOnomatopeia() { }


    private void FixedUpdate()
    {
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
