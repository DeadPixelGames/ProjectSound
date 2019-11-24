using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
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


    private Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        animate = GetComponent<Animator>();
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
                Item item = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>().GetActiveItem();
                if(item != null)
                {
                    GameObject bubble = GameObject.Instantiate(item.itemEntityPrefab);
                    Vector3 pos = this.transform.position;
                    if (behaviour.facingLeft)
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
                    animate.SetBool("Throw", true);
                }
                
            }
            
            
            jump = Input.GetButton("Jump");
        }
    }

    private void FixedUpdate()
    {
        behaviour.Move(move * Time.fixedDeltaTime);
        animate.SetFloat("Speed", move);
        animate.SetBool("Jump", jump);
        
        
        behaviour.changeLayer(moveZ);
        moveZ = 0;
        if (jump)
        {
            behaviour.jump();
        }
        //jump = false;
        //jumpButton.pressed = false;
    }
    

}
