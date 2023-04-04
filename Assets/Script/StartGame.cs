using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public GameObject door1;
    public GameObject door2;

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            door1.SetActive(false);
            door2.SetActive(true);
            enemySpawner.enabled = true;
        }
    }
}
