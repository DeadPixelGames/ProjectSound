using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceController : MonoBehaviour
{
    public static InterfaceController Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

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
