using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Singleton
    public static GameManager instance;

    private static void InitSingleton(GameManager thisInstance) {
        if(instance != null && instance != thisInstance) {
            throw new System.Exception("Hay al menos dos instancias de " + thisInstance.GetType().Name);
        } else {
            instance = thisInstance;
        }
    }
    #endregion

    public Player player;
    
    public event System.Action<bool> onPauseChanged;

    private bool paused;

    #region Unity
    void Awake() {
        GameManager.InitSingleton(this);
        this.player.onPlayerDead += this.OnPlayerDead;
    }

    void Update() {
        if(Input.GetButtonDown("Pause")) {
            this.SetPaused(!this.paused);
        }
    }
    #endregion

    #region Getters & Setters
    public bool IsPaused() {
        return paused;
    }

    public void SetPaused(bool paused) {
        this.paused = paused;
        this.onPauseChanged.Invoke(paused);
    }
    #endregion

    private void OnPlayerDead() {
        //TODO Ir al menú de selección de niveles
    }

    #region Music
    public void PlayMusic(AudioClip audio) {
        //TODO Reproducir AudioSource de Camera.main
    }

    public void StopMusic() {
        //TODO Detener AudioSource de Camera.main
    }
    #endregion
}
