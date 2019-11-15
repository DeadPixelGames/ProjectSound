using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    //Vida
    public int health;

    //Si el personaje está tocando el suelo
    bool grounded = false;

    // Objeto que determina el "collider" de cuando está en el suelo
    [SerializeField] private Transform groundCheck;

    // Capa(s) de los objetos que se consideran suelo
    [SerializeField] private LayerMask whatIsGround;

    private void Awake() { }

    /* Método para mover al personaje */
    public void move(float movement, bool jump) { }

    /* Método para quitar vida */
    public void takeDmg(int dmg) { this.health -= dmg; }
}
