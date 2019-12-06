using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>
    Singleton component managing and exposing global aspects of the game.
    </summary>
*/
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
    
    public Joystick joystick;
    public ButtonController jumpButton;
    public ButtonController actionButton;

    public float deathCounter;
    
    /** <summary>
     * Represents if operating in a handheld device
     * </summary>
    */
    public static bool operatingInMobile = false;

    /** <summary>
        Reference to the player entity.
        </summary>
    */
    public Player player;

    /** <summary>
        List containing the Z-coordinate of all layers available in the stage.
        </summary>
    */
    public List<GameObject> layers;

    /** <summary>
        Event that triggers whenever transitioning into or out of the pause state.
        </summary>
    */    
    public event System.Action<bool> onPauseChanged;

    /** <summary>
        Indicates whether the game is paused.
        </summary>
    */
    private bool paused;

    #region Unity
    void Awake() {
        GameManager.InitSingleton(this);
        this.player.onPlayerDead += this.OnPlayerDead;
         
        #region Joystick and Buttons in Mobile

        if (SystemInfo.operatingSystem.Split(' ')[0].Equals("Android"))
        {
            operatingInMobile = true;
            jumpButton.gameObject.SetActive(true);
            actionButton.gameObject.SetActive(true);
            joystick.gameObject.SetActive(true);
        }
        else
        {
            operatingInMobile = false;
            jumpButton.gameObject.SetActive(false);
            actionButton.gameObject.SetActive(false);
            joystick.gameObject.SetActive(false);
        }
        #endregion
    }

    void Update() {
        if(Input.GetButtonDown("Pause")) {
            this.SetPaused(!this.paused);
        }
    }
    #endregion

    #region Getters & Setters
    /** <summary>
        Returns whether the game is paused.
        </summary>
    */
    public bool IsPaused() {
        return paused;
    }

    /** <summary>
        Sets the paused state of the game, and triggers the event if changed.
        </summary>
    */
    public void SetPaused(bool paused) {
        if(paused != this.paused && this.onPauseChanged != null) {
            this.onPauseChanged.Invoke(paused);
        }
        this.paused = paused;
    }

    /** <summary>
        Returns the Z-coordinate of the indicated existing layer, or the nearest existing layer
        if the given layer does not exist.
        </summary>
    */
    public float GetLayer(int layer) {
        var listIndex = ClampLayer(layer);
        return this.layers[listIndex].transform.position.z;
    }
    #endregion

    private void OnPlayerDead() {
        this.StartCoroutine(this.DeathCoroutine());
    }

    private IEnumerator DeathCoroutine() {
        yield return new WaitForSeconds(this.deathCounter);
        //TODO Ir al menú de selección de niveles
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public int ClampLayer(int layer)
    {
        return Mathf.Clamp(layer, 0, this.layers.Count - 1);
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
