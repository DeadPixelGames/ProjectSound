using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** <summary>
        Component indicating an item as part of the game world. This is different from a regular
        Item object in that it behaves like a physical entity that can interact with other
        entities.
        </summary>
    */
public class ItemEntity : Entity, IActionable {
    /** <summary>
        Item object that this entity will turn into if put away into the inventory.
        </summary>
    */
    public Item item;

    protected bool floating = true;

    private new Collider collider;

    protected override void Awake() {
        base.Awake();
        this.collider = this.GetComponent<Collider>();
    }

    protected override void Update() {
        base.Update();
        this.rigidbody.isKinematic = this.floating;  
        this.collider.isTrigger = this.floating;

        if(this.getHealth() <= 0) {
            GameObject.Destroy(this.gameObject);
        }
    }

    /** <summary>
        Adds the item to the inventory, and removes this entity from the world.
        </summary>
    */
    public void Grab() {
        Inventory.instance.AddItem(this.item);
        GameObject.Destroy(this);
    }

    public override void Move(float move) {
        // Nothing. The point of this overriding is to remove the requirement to add an
        // implementation of this method, as it should be optional for any ItemEntity-derived class
    }

    public virtual void Use(int direction, Vector3 position) {
        // Nothing
    }

    void IActionable.Action()
    {
        Inventory.instance.AddItem(item);
        GameObject.Destroy(this.gameObject);
    }
}
