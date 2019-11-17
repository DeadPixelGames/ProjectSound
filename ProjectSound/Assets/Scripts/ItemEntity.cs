using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : Entity {
    
    public Item item;

    public void Grab() {
        Inventory.instance.AddItem(this.item);
    }

    protected override void Move() {
        // Nada, la presencia de esta sobreescritura es eliminar la obligatoriedad de implementar
        // el método Move() abstracto en clases derivadas
    }

    protected virtual void Use() {
        // Nada
    }
}
