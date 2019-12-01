using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CATALOGO_handler : MonoBehaviour
{

    [SerializeField] private Animator catalogo_animator;


    public void pasarPaginaDerecha()
    {
        catalogo_animator.SetBool("pagina1", true);
        catalogo_animator.SetBool("pagina2", false);
        Debug.Log("HOLA");
    }

    public void pasarPaginaIzquierda()
    {
        Debug.Log("Epaaaaaa");
        catalogo_animator.SetBool("pagina1", false);
        catalogo_animator.SetBool("pagina2", true);
    }
}
