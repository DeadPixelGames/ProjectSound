using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashSpawner : BubbleSpawner
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Spawn();
        }
    }


    
}
