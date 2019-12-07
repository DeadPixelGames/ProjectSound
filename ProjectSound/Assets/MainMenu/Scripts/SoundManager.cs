using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager instance;

    private static void InitSingleton(SoundManager thisInstance)
    {
        if (instance != null && instance != thisInstance)
        {
            throw new System.Exception("Hay al menos dos instancias de " + thisInstance.GetType().Name);
        }
        else
        {
            instance = thisInstance;
        }
    }
    #endregion


    private void Awake()
    {
        InitSingleton(this);
    }


    [SerializeField] private GameObject notebook;
    [SerializeField] private GameObject catalogue;
    [SerializeField] private GameObject myCamera;

    public GameObject getNotebook()
    {
        return notebook;
    }
    public GameObject getCatalogue()
    {
        return catalogue;
    }
    public GameObject getMyCamera()
    {
        return myCamera;
    }
}
