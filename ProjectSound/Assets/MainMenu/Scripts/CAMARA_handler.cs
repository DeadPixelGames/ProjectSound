using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAMARA_handler : MonoBehaviour
{
    [SerializeField] private Animator camara_animator;

    public void irEstanteria()
    {
        camara_animator.SetBool("enEstanteria", true);
        Debug.Log("A la estanteria");
    }

public void irCorcho()
    {
        camara_animator.SetBool("enCorcho", true);
        Debug.Log("A la mesa");
    }

public void EstanteriaMesa()
    {
        camara_animator.SetBool("enEstanteria", false);
        Debug.Log("A la mesa");
    }


    public void corchoaMesa()
    {
        camara_animator.SetBool("enCorcho", false);
        Debug.Log("A la mesa");
    }

    public void mesaCatalogo()
    {
        camara_animator.SetBool("enCatalogo", true);
        Debug.Log("Al catalogo");
    }

    public void catalogoMesa()
    {
        camara_animator.SetBool("enCatalogo", false);
        Debug.Log("A la mesa");
    }
}

