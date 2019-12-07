using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEnd : MonoBehaviour
{

    
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject == GameManager.instance.player.gameObject) {
            //TODO Pantalla de victoria
            Time.timeScale = 0;
            InterfaceController.instance.setActiveVictory(true);
        }
    }
}
