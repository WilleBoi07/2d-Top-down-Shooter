using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<Collider2D> spawnAreas = new List<Collider2D>();  // List of spawn area colliders
    public Transform player;      // Reference to the player (still needed for AI)
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
            // Choose a random collider from the list
            Collider2D randomCollider = spawnAreas[Random.Range(0, spawnAreas.Count)];
            // Spawn within the random spawn area collider
            Vector2 spawnPosition = GetRandomPositionInCollider(randomCollider);
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

    // Function to get a random position within the bounds of the given collider
    private Vector2 GetRandomPositionInCollider(Collider2D collider)
    {
        // Get the bounds of the collider (x, y position and size)
        Bounds bounds = collider.bounds;

        // Generate a random position within the bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }
}
