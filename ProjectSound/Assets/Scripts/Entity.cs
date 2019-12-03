using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>
    Component controlling any GameObject that can move through the stage.
    </summary>
*/
public abstract class Entity : MonoBehaviour {
    
    protected int layer;

    protected bool snapToLayer = true;

    #region Unity
    protected void Awake() {
        this.health = this.maxHealth;
    }

    protected void Update() {
        // Nothing
    }

    protected void FixedUpdate() {
        if(!GameManager.instance.IsPaused()) {
            this.Move(0);
        }
        if(this.snapToLayer) {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, GameManager.instance.GetLayer(this.layer));
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
    private float health;
    public float maxHealth;

    private float invulnerabilityTime;

    public float getHealth() { return health; }
    public void setHealth(float h) { Mathf.Clamp(h, 0, maxHealth); }
    public void addHealth(float h) { Mathf.Clamp(h + health, 0, maxHealth); }

    public int GetLayer() {
        return this.layer;
    }

    public void jump() { }
}
