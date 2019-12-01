using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUADERNO_handler : MonoBehaviour
{


    [SerializeField] private Animator cuaderno_animator;

    public void salir()
    {
        cuaderno_animator.SetBool("salir", true);
        Debug.Log("Saliendo");
    }

}
