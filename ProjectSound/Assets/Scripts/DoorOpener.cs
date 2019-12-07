using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour, IZappable {
    
    public Animator door;
    public GameObject luzRoja;
    public GameObject luzVerde;
    public GameObject luzPuertaRoja;
    public GameObject luzPuertaVerde;

    private void Start()
    {
        luzRoja.gameObject.SetActive(false);
        luzVerde.gameObject.SetActive(true);
        luzPuertaRoja.gameObject.SetActive(true);
        luzPuertaVerde.gameObject.SetActive(false);
    }

    public void Zap() {
        this.door.SetTrigger("Open");
        luzRoja.gameObject.SetActive(true);
        luzVerde.gameObject.SetActive(false);
        luzPuertaRoja.gameObject.SetActive(false);
        luzPuertaVerde.gameObject.SetActive(true);
    }

}
