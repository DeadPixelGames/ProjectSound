using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour, IZappable {
    
    public Animator door;

    public void Zap() {
        this.door.SetTrigger("Open");
    }

}
