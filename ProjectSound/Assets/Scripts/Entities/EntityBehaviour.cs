using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityBehaviour : MonoBehaviour
{
    //Velocidad
    public float speed;

    //Evento de inicio de colisión
    public UnityEvent onCollisionStart;

    //Evento de mantenerse en colisión
    public UnityEvent onCollisionStay;

    //Evento de salir de colisión
    public UnityEvent onCollisionEnd;

    /* Método de movimiento de la entidad */
    public void move(float movement)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        onCollisionStart.Invoke();   
    }

    private void OnCollisionStay(Collision collision)
    {
        onCollisionStay.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        onCollisionEnd.Invoke();
    }
}
