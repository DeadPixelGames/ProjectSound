using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    
    public float minActivationDistance = 5f;

    public EnemyController enemyPrefab;

    public int layer;

    private EnemyController lastSpawnedEnemy;

    private void Update() {
        if(Vector2.Distance(this.transform.position, GameManager.instance.player.transform.position) >= this.minActivationDistance && this.CanSpawn()) {
            this.lastSpawnedEnemy = GameObject.Instantiate(this.enemyPrefab.gameObject, this.transform.position, Quaternion.Euler(0, Random.value < 0.5 ? 90 : 270, 0)).GetComponent<EnemyController>();
            this.lastSpawnedEnemy.SetLayer(this.layer);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(this.transform.position, this.minActivationDistance);
    }

    private bool CanSpawn() {
        return this.lastSpawnedEnemy == null;
    }
}
