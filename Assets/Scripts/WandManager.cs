using UnityEngine;

public class WandManager : MonoBehaviour
{
    [Header("Wand Settings")]
    public float offsetRadius;
    public Transform towerTransform;
    public Wand wandPrefab;
    public Vector3 wandSpawnOffset;
    [SerializeField] public GameObject canvasWandInventoryContainer;


    // Removed the spawn call from Start()
    // Now we provide a public method to spawn the wand after tower is assigned.
    public void SpawnInitialWand()
    {
        if (towerTransform == null)
        {
            Debug.LogWarning("TowerTransform is not set on WandManager!");
            return;
        }

        wandSpawnOffset = new Vector3(offsetRadius, 0f, 0f);
        Vector3 spawnPosition = towerTransform.position + wandSpawnOffset;
        var currentWand = Instantiate(wandPrefab, spawnPosition, Quaternion.identity);
        currentWand.towerTransform = towerTransform;
    }
}
