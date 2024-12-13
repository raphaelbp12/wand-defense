using UnityEngine;

[RequireComponent(typeof(EnemySpawner))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Tower towerPrefab;
    private EnemySpawner enemySpawner;

    private Tower currentTower;

    private void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        SpawnTower();
    }

    private void SpawnTower()
    {
        // Instantiates the Tower at the origin
        currentTower = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity);

        // Assigns the Tower’s transform to the EnemySpawner so enemies know where to go
        enemySpawner.centerPoint = currentTower.transform;
    }

    public void RestartGame()
    {
        // Logic to reset the game.
        // For a quick test, just reload the current scene:
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        // Or implement another logic to clear enemies, respawn tower, etc.
    }
}
