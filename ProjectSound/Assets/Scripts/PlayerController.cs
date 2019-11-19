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

    // Start is called before the first frame update
    void Start()
    {
     
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.operatingInMobile)
        {
            move = joystick.Horizontal;
            //moveZ = joystick.Vertical;
            jump = jumpButton.pressed;
        }
        else
        {
            move = Input.GetAxisRaw("Horizontal");
            moveZ = Input.GetAxisRaw("Vertical");
            
            jump = Input.GetButton("Jump");
        }
    }

    private void FixedUpdate()
    {
        behaviour.move(move * Time.fixedDeltaTime);
        behaviour.changeLayer(moveZ);
        if (jump)
        {
            behaviour.jump();
        }
        //jump = false;
        //jumpButton.pressed = false;
    }
    

}
