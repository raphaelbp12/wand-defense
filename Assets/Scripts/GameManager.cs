using UnityEngine;

[RequireComponent(typeof(EnemySpawner), typeof(WaveManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Tower towerPrefab;

    private EnemySpawner enemySpawner;
    private WaveManager waveManager;
    private Tower currentTower;

    private void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        waveManager = GetComponent<WaveManager>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        SpawnTower();
        StartRound();
    }

    private void SpawnTower()
    {
        currentTower = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity);
        enemySpawner.centerPoint = currentTower.transform;

        // Subscribe to the tower's death event
        TowerDeathHandler deathHandler = currentTower.GetComponent<TowerDeathHandler>();
        if (deathHandler != null)
        {
            deathHandler.OnTowerDied.AddListener(HandleTowerDeath);
        }
    }

    public void StartRound()
    {
        waveManager.StartRound();
    }

    private void HandleTowerDeath()
    {
        // Tower has died, end the game and restart
        EndGame();
        RestartGame();
    }

    public void EndGame()
    {
        Debug.Log("Game Over. Handle UI and score finalization here.");
        // Stop spawning waves, show Game Over UI, etc.
    }

    public void RestartGame()
    {
        Debug.Log("Restarting the game...");
        // For a quick restart, reload the current scene:
        // UnityEngine.SceneManagement.SceneManager.LoadScene(
        //     UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        // );

        // Or implement custom logic to clear enemies, reset state, and call StartRound() again.
    }
}
