using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject enemyTwoPrefab;
    [SerializeField]
    private GameObject enemyThreePrefab;

    [SerializeField]
    private float enemyInterval = 3.5f;
    [SerializeField]
    private float enemyTwoInterval = 7f;
    [SerializeField]
    private float enemyThreeInterval = 14f;

    [SerializeField]
    private int maxEnemyCount = 20; // Max limit of enemies at a time
    private int currentEnemyCount = 0; // To track current enemies in the scene

    // Define spawn area
    [SerializeField]
    private Vector2 spawnAreaMin = new Vector2(-5f, -6f);
    [SerializeField]
    private Vector2 spawnAreaMax = new Vector2(5f, 6f);

    // Optional: Visualize spawn area in editor
    [SerializeField]
    private bool showSpawnArea = true;

    void Start()
    {
        StartCoroutine(spawnEnemy(enemyInterval, enemyPrefab));
        StartCoroutine(spawnEnemy(enemyTwoInterval, enemyTwoPrefab));
        StartCoroutine(spawnEnemy(enemyThreeInterval, enemyThreePrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemyPrefab)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            
            // Check if we can spawn more enemies
            if (currentEnemyCount < maxEnemyCount)
            {
                Vector3 spawnPos = GetRandomSpawnPosition();
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                // Increase the current enemy count
                currentEnemyCount++;

                // Subscribe to the OnDestroyed event to decrement the count when an enemy is destroyed
                Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.OnDestroyed += () => currentEnemyCount--;
                }
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector3(x, y, 0);
    }

    // Visualize spawn area in editor
    void OnDrawGizmos()
    {
        if (showSpawnArea)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(
                new Vector3((spawnAreaMin.x + spawnAreaMax.x) / 2, (spawnAreaMin.y + spawnAreaMax.y) / 2, 0),
                new Vector3(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y, 0)
            );
        }
    }
}
