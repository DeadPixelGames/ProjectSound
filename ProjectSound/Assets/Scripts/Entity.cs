using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>
    Component controlling any GameObject that can move through the stage.
    </summary>
*/
public abstract class Entity : MonoBehaviour {
    
    [SerializeField]
    protected int layer;

    protected Rigidbody rb;

    protected bool snapToLayer = true;

    #region Unity
    protected virtual void Awake() {
        this.health = this.maxHealth;
        this.rb = this.GetComponent<Rigidbody>();
    }

    protected virtual void Update() {
        // Nothing
    }

    protected virtual void FixedUpdate() {
        if(!GameManager.instance.IsPaused()) {
            this.Move(0);
        }
        if(this.snapToLayer) {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, GameManager.instance.GetLayer(this.layer));
        }
        if(this.transform.position.y < GameManager.instance.bottomlessPit.position.y) {
            this.setHealth(0);
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
    public void setHealth(float h) { this.health = Mathf.Clamp(h, 0, maxHealth); }
    public void addHealth(float h) { this.health = Mathf.Clamp(h + health, 0, maxHealth); }

    public int GetLayer() {
        return this.layer;
    }

    public void SetLayer(int layer) {
        this.layer = layer;
    }

    public void jump() { }
}
