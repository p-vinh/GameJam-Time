using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; 
    public int maxSpawnCount = 20;
    private int currentSpawnCount;
    public float despawnRadius = 20f;
    public float spawnInterval = 1f;
    GameObject[] enemyArray;
    private float spawnPadding = 5f;
    [SerializeField] TimeController controller;

    private void Start() {
        StartCoroutine(SpawnEnemies(spawnInterval, enemyPrefabs));
    }

    private void Update() {
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy")
            .Where(e => e.transform.parent == null || e.transform.parent.tag != "Enemy").ToArray();

        foreach (GameObject enemy in enemyArray)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) > despawnRadius)
            {
                controller.RemoveObject(enemy);
                Destroy(enemy);
            }
        }
    }

    //fIx this max it sothat it spawns outside of viewport
    IEnumerator SpawnEnemies(float interval, GameObject[] enemies) {
        yield return new WaitForSeconds(interval);
        currentSpawnCount = enemyArray.Length;

        if (currentSpawnCount < maxSpawnCount) {
            // Get the camera's viewport bounds
            Camera cam = Camera.main;
            float camHeight = 2f * cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;
            Rect viewportBounds = new Rect(cam.transform.position.x - camWidth / 2f, cam.transform.position.y - camHeight / 2f, camWidth, camHeight);


            // Randomly generate a point outside of the viewport
            Vector3 spawnPoint = Vector3.zero;
            bool pointInsideViewport = true;
            while (pointInsideViewport)
            {
                spawnPoint = new Vector3(Random.Range(viewportBounds.xMin - spawnPadding, viewportBounds.xMax + spawnPadding), Random.Range(viewportBounds.yMin - spawnPadding, viewportBounds.yMax + spawnPadding), 0f);
                pointInsideViewport = viewportBounds.Contains(spawnPoint);
            }

            // Spawn the enemy at the spawn point
            int index = Random.Range(0, enemies.Length);
            GameObject newEnemy = Instantiate(enemies[index], spawnPoint, Quaternion.identity);
            controller.ObjectToTime(newEnemy);
        }
        StartCoroutine(SpawnEnemies(spawnInterval, enemyPrefabs));
    }
}
