using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>
    Component controlling the entity representing the player character.
    </summary>
*/
public class Player : Entity {

    /** <summary>
        Event that triggers when the player dies.
        </summary>
    */
    public event System.Action onPlayerDead;

    protected override void Move() {
        //TODO Quitar esto en el merge
        this.onPlayerDead.Invoke(); 
    }
}
