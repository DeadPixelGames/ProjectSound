using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceController : MonoBehaviour
{
    #region Singleton
    public static InterfaceController instance;

    private static void InitSingleton(InterfaceController thisInstance)
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

    [SerializeField] private GameObject inputUIElements;
    [SerializeField] private GameObject gameOverUIElements;
    [SerializeField] private GameObject victoryUIElements;
    [SerializeField] private GameObject pauseUIElements;

    [SerializeField] private AudioClip openMenu;
    [SerializeField] private AudioClip buttonPressed;

    private AudioSource audioSource;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
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

    public void RestartScene()
    {
        GameManager.instance = null;
        Inventory.instance = null;
        instance = null;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartScene(string scene)
    {

        GameManager.instance = null;
        Inventory.instance = null;
        instance = null;
        Time.timeScale = 1;
        StayThroughScenesObject.instance.setSceneIWantToLoad(scene);
        SceneManager.LoadScene("LoadingScene");
    }

    public void SetLvlSelection(bool lvlSel)
    {
        StayThroughScenesObject.instance.setLvLSelection(lvlSel);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void setActiveGameOver(bool active)
    {
        audioSource.clip = openMenu;
        audioSource.Play();

        gameOverUIElements.SetActive(active);
    }

    public void setActiveVictory(bool active)
    {
        audioSource.clip = openMenu;
        audioSource.Play();
        victoryUIElements.SetActive(active);
    }

    public void setActivePause(bool active)
    {
        audioSource.clip = openMenu;
        audioSource.Play();
        pauseUIElements.SetActive(active);
    }

    public void setActiveInput(bool active)
    {
        inputUIElements.SetActive(active);
    }


}
