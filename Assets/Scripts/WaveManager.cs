using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int totalWaves = 3;
    public float timeBetweenWaves = 5f;

    private EnemySpawner enemySpawner;
    private bool roundInProgress = false;

    // Track current wave and how many enemies are alive in that wave.
    private int currentWaveIndex = -1;
    private int enemiesAlive = 0;

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
            currentWaveIndex = i;

            // Spawn the current wave and get the number of enemies spawned
            enemiesAlive = enemySpawner.SpawnWave(i);

            // Wait until the current wave is cleared or timeBetweenWaves is over to spawn the next wave
            // Instead of just waiting a fixed time, you might wait for enemiesAlive to hit 0 
            // or use timeBetweenWaves as a delay before the next wave. For simplicity, we do:
            if (i < totalWaves - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        roundInProgress = false;
    }

    public void EnemyDefeated()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            // Check if it was the last wave
            if (currentWaveIndex == totalWaves - 1)
            {
                WinGame();
            }
            else
            {
                // If not the last wave, the next wave will start after timeBetweenWaves passes in the coroutine
                // or you can handle immediate next wave start here if desired.
            }
        }
    }

    private void WinGame()
    {
        // Load WinScene
        SceneManager.LoadScene("WinScene");
    }
}
