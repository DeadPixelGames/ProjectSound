using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSpawner : BubbleSpawner
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Spawn();
        }
    }


    
}
