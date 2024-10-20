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

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        Vector3 spawnPos = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
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