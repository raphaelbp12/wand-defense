using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameSceneManager))]
public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int totalWaves = 3;
    public float timeBetweenWaves = 5f;

    private GameSceneManager gameSceneManager;
    private EnemySpawner enemySpawner;
    private bool roundInProgress = false;

    // Array that tracks how many enemies remain in each wave.
    private int[] waveEnemiesRemaining;

    // Keep track of which wave is currently spawning
    private int currentWaveIndex = -1;

    private void Awake()
    {
        gameSceneManager = GetComponent<GameSceneManager>();
        enemySpawner = GetComponent<EnemySpawner>();

        // Initialize array to hold remaining enemy counts per wave.
        // If you have different spawn amounts for each wave, you might fill this array dynamically.
        waveEnemiesRemaining = new int[totalWaves];
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
            currentWaveIndex = i;

            // Spawn the current wave and get the number of enemies spawned.
            int spawnedCount = enemySpawner.SpawnWave(i);
            waveEnemiesRemaining[i] = spawnedCount;

            // Wait until timeBetweenWaves is over (or the wave is cleared) before spawning the next wave
            if (i < totalWaves - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        roundInProgress = false;
    }

    /// <summary>
    /// Called by the Enemy or EnemyDeathHandler, passing in the wave index that enemy belonged to.
    /// </summary>
    public void EnemyDefeated(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waveEnemiesRemaining.Length) return;

        waveEnemiesRemaining[waveIndex]--;

        // If there are no more enemies in that wave,
        // check if it's the last wave to trigger the win condition.
        if (waveEnemiesRemaining[waveIndex] <= 0)
        {
            // If this was the last wave
            if (waveIndex == totalWaves - 1)
            {
                WinRound();
            }
            // Otherwise, you can decide if you want to do something else,
            // such as immediately start the next wave (depending on your design).
        }
    }

    private void WinRound()
    {
        gameSceneManager.WinRound();
    }
}
