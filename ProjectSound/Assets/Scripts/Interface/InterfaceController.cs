using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceController : MonoBehaviour
{

    /* Esconde el objeto */
    public void hide(GameObject go)
    {
        go.SetActive(false);
    }
    /* Muestra el objeto */
    public void show(GameObject go)
    {
        go.SetActive(true);
    }
}
