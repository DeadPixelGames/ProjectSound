using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>
    Component controlling any GameObject that can move through the stage.
    </summary>
*/
public abstract class Entity : MonoBehaviour {
    
    #region Unity
    private void FixedUpdate() {
        if(!GameManager.instance.IsPaused()) {
            this.Move(0);
        }
    }
    #endregion

    /** <summary>
        Describes how the entity should behave while the game is running.
        </summary>
    */
    public abstract void Move(float move);

    //Velocidad
    public float walkingSpeed;
    public float jumpSpeed;

    //Vida
    private float health = 3;
    public float maxHealth = 3;

    private float invulnerabilityTime;

    public float getHealth() { return health; }
    public void setHealth(float h) { Mathf.Clamp(h, 0, maxHealth); }
    public void addHealth(float h) { health += h; }

    public void jump() { }
}
