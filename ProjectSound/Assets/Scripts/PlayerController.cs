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

    public Animator animate;

    private Rigidbody rigidBody;

    private Inventory inventory;

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
            move = joystick.Horizontal;
            moveZ = joystick.Vertical;
            jump = jumpButton.pressed;
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
                    animate.SetTrigger("Throw");
                }
                
            }
            jump = Input.GetButton("Jump");
        }
    }

    private void FixedUpdate()
    {

        Debug.Log(move);
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
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, this.actionRadius);
        Collider closest = null;
        foreach(Collider collider in colliders)
        {
            if (!closest)
            {
                if((this.transform.position - collider.gameObject.transform.position).magnitude < (this.transform.position - closest.transform.position).magnitude)
                {
                    closest = collider;
                }
            }
            else
            {
                closest = collider;
            }
        }
        if (!closest)
        {
            IActionable actionableComponent = closest.gameObject.GetComponent<IActionable>();
            actionableComponent.Action();
        }
        

    }

}
