using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Enemy enemyPrefab;
    public int enemiesPerWave = 5;
    public Transform centerPoint;
    public float minSpawnRadius = 5f;
    public float maxSpawnRadius = 10f;

    private int currentWaveIndex = 0;

    private void Update()
    {
        // Check if any key was pressed this frame
        if (Input.anyKeyDown)
        {
            SpawnWave(currentWaveIndex);
            currentWaveIndex++;
        }
    }

    public void SpawnWave(int waveIndex)
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPoint();
            Enemy newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.GetComponent<EnemyMovementController>().towerTransform = centerPoint;
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        // Pick a random angle in radians
        float angle = Random.Range(0f, Mathf.PI * 2f);
        
        // Pick a random distance between min and max radius
        float distance = Random.Range(minSpawnRadius, maxSpawnRadius);

        // Convert from polar to Cartesian coordinates
        float x = centerPoint.position.x + Mathf.Cos(angle) * distance;
        float y = centerPoint.position.y + Mathf.Sin(angle) * distance;

        // Assuming a flat plane (z is fixed); adjust z if needed
        float z = centerPoint.position.z;

        return new Vector3(x, y, z);
    }
}
