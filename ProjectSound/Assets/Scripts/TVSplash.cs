using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVSplash : MonoBehaviour, ISplashable {

    private BubbleSpawner spawner;

    private void Awake() {
        this.spawner = this.GetComponent<BubbleSpawner>();
    }

    public void Splash() {
        this.spawner.Spawn();
    }
}
