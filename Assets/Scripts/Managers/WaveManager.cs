using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(GameSceneManager))]
public class WaveManager : MonoBehaviour
{
    public TMPro.TMP_Text roundWaveInfoText;

    [Header("Wave Settings")]
    public int wavesPerRound = 3;
    private int lastWaveThisRound = 0;
    private int currentWaveIndex = 0;
    public float timeBetweenWaves = 10f;

    private GameSceneManager gameSceneManager;
    private EnemySpawner enemySpawner;
    private bool roundInProgress = false;

    // Array that tracks how many enemies remain in each wave.
    private Dictionary<int, int> waveEnemiesRemaining;

    private void Awake()
    {
        gameSceneManager = GetComponent<GameSceneManager>();
        enemySpawner = GetComponent<EnemySpawner>();

        // Initialize array to hold remaining enemy counts per wave.
        // If you have different spawn amounts for each wave, you might fill this array dynamically.
        waveEnemiesRemaining = new Dictionary<int, int>();
    }

    private void Update()
    {
        if (roundWaveInfoText != null)
        {
            roundWaveInfoText.text = $"Wave: {GlobalData.Instance.currentWaveIndex}";
        }
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
        lastWaveThisRound = GlobalData.Instance.currentWaveIndex + wavesPerRound;
        currentWaveIndex = GlobalData.Instance.currentWaveIndex;
        for (int i = currentWaveIndex; i < lastWaveThisRound; i++)
        {
            // Spawn the current wave and get the number of enemies spawned.
            int spawnedCount = enemySpawner.SpawnWave(i);
            waveEnemiesRemaining[i] = spawnedCount;

            // Wait until timeBetweenWaves is over (or the wave is cleared) before spawning the next wave
            if (i < lastWaveThisRound - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
                GlobalData.Instance.IncrementCurrentWaveIndex();
            }
        }

        roundInProgress = false;
    }

    /// <summary>
    /// Called by the Enemy or EnemyDeathHandler, passing in the wave index that enemy belonged to.
    /// </summary>
    public void EnemyDefeated(int waveIndex)
    {
        if (!waveEnemiesRemaining.ContainsKey(waveIndex)) return;

        waveEnemiesRemaining[waveIndex]--;

        // If there are no more enemies in that wave,
        // check if it's the last wave to trigger the win condition.
        if (waveEnemiesRemaining[waveIndex] <= 0)
        {
            // If this was the last wave
            if (waveIndex == lastWaveThisRound - 1)
            {
                WinRound();
            }
            // Otherwise, you can decide if you want to do something else,
            // such as immediately start the next wave (depending on your design).
        }
    }

    private void WinRound()
    {
        GlobalData.Instance.IncrementCurrentWaveIndex();
        gameSceneManager.WinRound();
    }
}
