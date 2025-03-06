using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawner;  // Reference to the spawner object
    public Transform player;   // Reference to the player (still needed for AI)
    public float spawnRadius = 5f;
    public int enemyCount = 4;
    public float spawnInterval = 7.5f; // Time between waves
    public bool isSpawning = false;

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnWaveRoutine());
        }
    }

    private IEnumerator SpawnWaveRoutine()
    {
        while (isSpawning)
        {
            SpawnWave();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopCoroutine(SpawnWaveRoutine());
    }

    private void SpawnWave()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Spawn around the spawner, not the player
            Vector2 spawnPosition = (Vector2)spawner.position + Random.insideUnitCircle.normalized * spawnRadius;
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Set up the enemy AI with player reference (keep player reference for AI behavior)
            EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.player = player;  // Set the player reference for chasing
                enemyAI.currentState = EnemyAI.EnemyState.Chasing;
                enemyAI.isWaveEnemy = true;
            }
        }
    }
}
