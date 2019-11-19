using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    //Velocidad
    public float walkingSpeed;
    public float jumpSpeed;

    //Vida
    private float health;
    public float maxHealth;

    private float invulnerabilityTime;

    public float getHealth() { return health; }
    public void setHealth(float h) { Mathf.Clamp(h, 0, maxHealth); }
    public void addHealth(float h) { health += h; }

    private void Update()
    {
        
    }

    public void move(float move)
    {
        Vector3 pos = transform.position;
        pos.x += health;

        transform.position = pos;
    }
    public void jump() { }
}
