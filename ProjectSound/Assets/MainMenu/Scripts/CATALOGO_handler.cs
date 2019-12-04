using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CATALOGO_handler : MonoBehaviour
{

    [SerializeField] private Animator catalogo_animator;
    public Button Derecha;
    public Button Izquierda;
 

            private void Start()
    {
        Izquierda.gameObject.SetActive(false);
        Derecha.gameObject.SetActive(true);
    }

    public void pasarPaginaDerecha()
    {
        Izquierda.gameObject.SetActive(true);
        Derecha.gameObject.SetActive(false);
        catalogo_animator.SetBool("pagina1", true);
        catalogo_animator.SetBool("pagina2", false);
        Debug.Log("HOLA");
    }

    public void pasarPaginaIzquierda()
    {
        Izquierda.gameObject.SetActive(false);
        Derecha.gameObject.SetActive(true);
        Debug.Log("Epaaaaaa");
        catalogo_animator.SetBool("pagina1", false);
        catalogo_animator.SetBool("pagina2", true);
    }
}
