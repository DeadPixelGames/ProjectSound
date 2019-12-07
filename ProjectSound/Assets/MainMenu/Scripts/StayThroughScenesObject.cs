using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayThroughScenesObject : MonoBehaviour
{
    #region Singleton
    public static StayThroughScenesObject instance;

    private static void InitSingleton(StayThroughScenesObject thisInstance)
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


    private bool lvlSelection = false;
    private string sceneIWantToLoad = "";


    private void Awake()
    {
        InitSingleton(this);
        DontDestroyOnLoad(this);
    }

    public void setLvLSelection(bool lvlSel) { this.lvlSelection = lvlSel; }
    public bool isLvlSelection() { return this.lvlSelection; }

    public void setSceneIWantToLoad(string scene) { this.sceneIWantToLoad = scene; }
    public string getSceneIWantToLoad() { return this.sceneIWantToLoad; }
}
