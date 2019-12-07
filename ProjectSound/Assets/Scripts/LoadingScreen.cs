using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

    private string loadingThisScene;

    [SerializeField] private Image progressBar;

    private void Awake()
    {
        Time.timeScale = 1;
        loadingThisScene = StayThroughScenesObject.instance.getSceneIWantToLoad();
        StartCoroutine(loadSceneAsync());
        progressBar.fillAmount = 0;
    }


    private IEnumerator loadSceneAsync()
    {
        
        yield return new WaitForSeconds(0.5f);
        AsyncOperation loadingSceneOperation = SceneManager.LoadSceneAsync(loadingThisScene);

        
        
        while (loadingSceneOperation.progress < 1)
        {
            progressBar.fillAmount = loadingSceneOperation.progress;

            yield return new WaitForEndOfFrame();
        }


        ;
        yield return null;
        
    }
}
