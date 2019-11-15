using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : CharacterBehaviour
{
    //Dirección de mirada del personaje
    bool facingRight = false;

    private void Awake()
    {
        
    }

    /* Método para dar la vuelta al sprite del personaje */
    public void flip() { }

    /* Método para usar una onomatopeya */
    public void useOnomatopeia() { }
}
