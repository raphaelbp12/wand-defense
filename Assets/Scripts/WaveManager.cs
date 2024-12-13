using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int totalWaves = 3;         // how many waves in one round
    public float timeBetweenWaves = 5f; // time gap between waves

    private EnemySpawner enemySpawner;
    private bool roundInProgress = false;

    private void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
    }

    public void StartRound()
    {
        if (!roundInProgress)
        {
            roundInProgress = true;
            StartCoroutine(RunRound());
        }
    }

    private IEnumerator RunRound()
    {
        for (int i = 0; i < totalWaves; i++)
        {
            // Spawn the current wave
            enemySpawner.SpawnWave(i);

            // Wait before spawning the next wave, unless it's the last wave
            if (i < totalWaves - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        roundInProgress = false;
    }
}
