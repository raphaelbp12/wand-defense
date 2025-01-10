using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EnemySpawner), typeof(WaveManager), typeof(WandManager))]
public class GameSceneManager : MonoBehaviour
{
    [Header("References")]
    public Tower towerPrefab;

    private EnemySpawner enemySpawner;
    private WaveManager waveManager;
    private WandManager wandManager;
    private Tower currentTower;

    private void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        waveManager = GetComponent<WaveManager>();
        wandManager = GetComponent<WandManager>();
    }

    private void Start()
    {
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

        // Assign the tower transform to the WandManager and spawn the initial wand
        wandManager.towerTransform = currentTower.transform;
        wandManager.SpawnInitialWand();
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
        SceneManager.LoadScene("GameOverScene");
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
