using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Enemy enemyPrefab;
    public int enemiesPerWave = 5;
    public Transform centerPoint;
    public float minSpawnRadius = 5f;
    public float maxSpawnRadius = 10f;

    public int SpawnWave(int waveIndex)
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPoint();
            Enemy newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.towerTransform = centerPoint;

            // Subscribe to enemy death event
            EnemyDeathHandler deathHandler = newEnemy.GetComponent<EnemyDeathHandler>();
            if (deathHandler != null)
            {
                deathHandler.OnEnemyDied += HandleEnemyDeath;
            }
        }

        // Return how many enemies were spawned
        return enemiesPerWave;
    }

    private Vector3 GetRandomSpawnPoint()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(minSpawnRadius, maxSpawnRadius);
        float x = centerPoint.position.x + Mathf.Cos(angle) * distance;
        float y = centerPoint.position.y + Mathf.Sin(angle) * distance;
        float z = centerPoint.position.z;
        return new Vector3(x, y, z);
    }

    private void HandleEnemyDeath()
    {
        // Get reference to WaveManager and notify that an enemy is defeated
        WaveManager waveManager = GetComponent<WaveManager>();
        if (waveManager != null)
        {
            waveManager.EnemyDefeated();
        }
    }
}
