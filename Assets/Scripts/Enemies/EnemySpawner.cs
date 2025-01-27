using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(WaveManager), typeof(CurrencyManager))]
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform centerPoint;
    public float minSpawnRadius = 5f;
    public float maxSpawnRadius = 10f;

    [Header("Enemy Types")]
    public List<EnemyTypeDefinition> allEnemyTypes; // Drag multiple SOs here

    [Header("Spawn Behavior")]
    public bool spawnAllAtOnce = true;
    public float spawnDuration = 5f;

    public int SpawnWave(int waveIndex)
    {
        int totalSpawned = 0;

        foreach (var enemyDef in allEnemyTypes)
        {
            // Only spawn if waveIndex >= waveStart
            if (waveIndex >= enemyDef.waveStart)
            {
                int count = CalculateEnemyCountForType(enemyDef, waveIndex);

                if (spawnAllAtOnce)
                {
                    for (int i = 0; i < count; i++)
                    {
                        SpawnSingleEnemy(enemyDef, waveIndex);
                        totalSpawned++;
                    }
                }
                else
                {
                    // If you want them spread out over time
                    StartCoroutine(SpawnEnemiesOverTime(enemyDef, waveIndex, count));
                    totalSpawned += count;
                }
            }
        }

        return totalSpawned;
    }

    private IEnumerator SpawnEnemiesOverTime(EnemyTypeDefinition def, int waveIndex, int count)
    {
        float interval = spawnDuration / count;
        for (int i = 0; i < count; i++)
        {
            SpawnSingleEnemy(def, waveIndex);
            yield return new WaitForSeconds(interval);
        }
    }

    private void SpawnSingleEnemy(EnemyTypeDefinition def, int waveIndex)
    {
        Vector3 spawnPos = GetRandomSpawnPoint();
        GameObject enemyObj = Instantiate(def.enemyPrefab, spawnPos, Quaternion.identity);
        Enemy newEnemy = enemyObj.GetComponent<Enemy>();
        if (newEnemy != null)
        {
            newEnemy.InitializeEnemy(def, waveIndex, centerPoint);

            EnemyDeathHandler deathHandler = newEnemy.GetComponent<EnemyDeathHandler>();
            if (deathHandler != null)
            {
                deathHandler.OnEnemyDied += HandleEnemyDeath;
            }
        }
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

    private int CalculateEnemyCountForType(EnemyTypeDefinition def, int waveIndex)
    {
        // Example formula, can be anything:
        return waveIndex + 3;
    }

    private void HandleEnemyDeath(int waveIndex)
    {
        WaveManager waveManager = GetComponent<WaveManager>();
        CurrencyManager currencyManager = GetComponent<CurrencyManager>();

        if (waveManager != null)
        {
            waveManager.EnemyDefeated(waveIndex);
        }

        if (currencyManager != null)
        {
            currencyManager.OnEnemyDied();
        }
    }
}
