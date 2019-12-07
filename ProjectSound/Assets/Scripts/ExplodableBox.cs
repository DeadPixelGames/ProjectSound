using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableBox : MonoBehaviour, IExplodable
{

    public float despawnTime = 3f;

    public void Explode() {
        this.StartCoroutine(this.ExplodeCoroutine());
    }

    public IEnumerator ExplodeCoroutine() {
        yield return new WaitForSeconds(this.despawnTime);
        GameObject.Destroy(this.gameObject);
    }
}
