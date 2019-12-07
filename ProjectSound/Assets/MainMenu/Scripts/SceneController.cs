using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string sceneToChange;

    public void changeScene()
    {
        if(sceneToChange != null)
        {
            StayThroughScenesObject.instance.setSceneIWantToLoad(sceneToChange);
            SceneManager.LoadScene("LoadingScene");
        }
    }
}
