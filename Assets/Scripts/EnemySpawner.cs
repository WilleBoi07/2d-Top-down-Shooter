using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
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
            Vector2 spawnPosition = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.GetComponent<EnemyAI>().player = player;
            newEnemy.GetComponent<EnemyAI>().currentState = EnemyAI.EnemyState.Chasing;
            newEnemy.GetComponent<EnemyAI>().isWaveEnemy = true;
        }
    }
}
