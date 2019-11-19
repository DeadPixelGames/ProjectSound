using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>
    Component controlling any GameObject that can move through the stage.
    </summary>
*/
public abstract class Entity : MonoBehaviour {
    
    #region Unity
    void Update() {
        if(!GameManager.instance.IsPaused()) {
            this.Move();
        }
    }
    #endregion

    /** <summary>
        Describes how the entity should behave while the game is running.
        </summary>
    */
    protected abstract void Move();

    //Velocidad
    public float walkingSpeed;
    public float jumpSpeed;

    //Vida
    private float health;
    public float maxHealth;

    private float invulnerabilityTime;

    public float getHealth() { return health; }
    public void setHealth(float h) { Mathf.Clamp(h, 0, maxHealth); }
    public void addHealth(float h) { health += h; }

    public void jump() { }
}
