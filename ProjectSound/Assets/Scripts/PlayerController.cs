using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float actionRadius = 2f;

    //Controla cuando se ha de saltar
    bool jump = false;
    //Cantidad de movimiento
    float move = 0f;

    float moveZ = 0f;
    //Instancia del joystick
    public Joystick joystick;
    //Instancia del botón de salto
    public ButtonController jumpButton;
    //Instancia del botón de acción
    public ButtonController actionButton;

    //Intancia del comportamiento del personaje
    public Player behaviour;

    private Animator animate;

    private Rigidbody rigidBody;

    private Inventory inventory;

    private GameObject closestInteractableObject;

    private bool movingThroughLayersUsingTheJoystick = false;


    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        animate = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.operatingInMobile)
        {
            if(joystick.Horizontal > 0.2 || joystick.Horizontal < -0.2)
            {
                move = joystick.Horizontal;
            }
            else
            {
                move = 0;
            }
            
            
            if(!movingThroughLayersUsingTheJoystick && joystick.Vertical > 0.7)
            {
                movingThroughLayersUsingTheJoystick = true;
                moveZ = 1;
            }else if(!movingThroughLayersUsingTheJoystick && joystick.Vertical < -0.7)
            {
                movingThroughLayersUsingTheJoystick = true;
                moveZ = -1;
            }
            else if(joystick.Vertical >= -0.7 && joystick.Vertical <= 0.7)
            {
                movingThroughLayersUsingTheJoystick = false;
            }
            
            jump = jumpButton.pressed;
            if (actionButton.pressed)
            {
                actionButton.pressed = false;
                Item item = Inventory.instance.GetActiveItem();
                if (item != null)
                {
                    if (item.isDrinkable())
                    {
                        animate.SetTrigger("Drink");
                    }
                    else
                    {
                        animate.SetTrigger("Throw");
                    }
                    
                }
                else
                {
                    animate.SetTrigger("Pick");
                }
            }
        }
         else
        {
            move = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Vertical"))
            moveZ = Input.GetAxisRaw("Vertical");
            if (Input.GetButtonDown("InventoryScroll"))
            {
                if(Input.GetAxisRaw("InventoryScroll") > 0)
                {
                    inventory.SetActiveItem(inventory.GetActiveItemIndex() + 1);
                }
                else if(Input.GetAxisRaw("InventoryScroll") < 0)
                {
                    inventory.SetActiveItem(inventory.GetActiveItemIndex() - 1);
                }
            }
            if (Input.GetButtonDown("Action"))
            {
                Item item = Inventory.instance.GetActiveItem();
                if(item != null)
                {
                    if (item.isDrinkable())
                    {
                        animate.SetTrigger("Drink");
                    }
                    else
                    {
                        animate.SetTrigger("Throw");
                    }
                }
                else
                {
                    animate.SetTrigger("Pick");
                }
                
            }
            jump = Input.GetButton("Jump");
        }
    }

    private void FixedUpdate()
    {

        checkClosestObject();
        if(closestInteractableObject != null)
        {
            GlowObjectCmd glow = closestInteractableObject.GetComponent<GlowObjectCmd>();
            if (glow != null)
            {
                glow.Glow();
            }
        }
        
        behaviour.Move(move * Time.fixedDeltaTime);
        animate.SetFloat("Speed", move);
        animate.SetBool("Jump", jump);
        
        animate.SetFloat("YSpeed", rigidBody.velocity.y);
        
        behaviour.changeLayer(moveZ);
        moveZ = 0;


    }

    
    public void setAnimatorGrounded()
    {
        animate.SetBool("Grounded", true);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, this.actionRadius);
    }

    public void OnAction()
    {
       
        if (closestInteractableObject != null)
        {
            IActionable actionableComponent = closestInteractableObject.GetComponent<IActionable>();
            actionableComponent.Action();
            
        }
        

    }



    private void checkClosestObject()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, this.actionRadius);
        Collider closest = null;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<IActionable>() != null)
            {
                if (closest != null)
                {
                    if ((this.transform.position - collider.gameObject.transform.position).magnitude < (this.transform.position - closest.transform.position).magnitude)
                    {
                        closest = collider;
                    }
                }
                else
                {
                    closest = collider;
                }
            }
        }
        if(closest != null)
        {
            if(closestInteractableObject != null)
            {
                if(closest.gameObject != closestInteractableObject)
                {
                    GlowObjectCmd glow = closestInteractableObject.GetComponent<GlowObjectCmd>();
                    if(glow != null)
                    {
                        glow.StopGlowing();
                    }
                    closestInteractableObject = closest.gameObject;
                }
            }
            else
            {
                closestInteractableObject = closest.gameObject;
            }
            
           
            
        }
        else
        {
            if(closestInteractableObject != null)
            {
                GlowObjectCmd glow = closestInteractableObject.GetComponent<GlowObjectCmd>();
                if (glow != null)
                {
                    glow.StopGlowing();
                }
            }
            closestInteractableObject = null;
        }
        
    }
}
