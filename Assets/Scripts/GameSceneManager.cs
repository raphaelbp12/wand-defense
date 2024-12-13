using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EnemySpawner), typeof(WaveManager))]
public class GameSceneManager : MonoBehaviour
{

    [Header("References")]
    public Tower towerPrefab;

    private EnemySpawner enemySpawner;
    private WaveManager waveManager;
    private Tower currentTower;

    private void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        waveManager = GetComponent<WaveManager>();
    }

    private void Start()
    {
        // If this is the main game scene, spawn tower and start round
        // If this is a menu scene, we won't do this.
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            SpawnTower();
            StartRound();
        }
    }

    private void SpawnTower()
    {
        currentTower = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity);
        enemySpawner.centerPoint = currentTower.transform;

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
        EndGame();
    }

    public void EndGame()
    {
        // Load the GameOver scene
        SceneManager.LoadScene("GameOverScene");
    }

    public void StartNewGame()
    {
        // Called from Main Menu UI button
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMainMenu()
    {
        // Called from Game Over UI button
        SceneManager.LoadScene("MainMenuScene");
    }
}
