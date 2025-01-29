using System.Collections.Generic;
using UnityEngine;

public class WandManager : MonoBehaviour
{
    [Header("Wand Settings")]
    public float offsetRadius;
    public Transform towerTransform;
    public Wand wandPrefab;
    public Vector3 wandSpawnOffset;
    [SerializeField] public GameObject canvasWandInventoryContainer;
    [SerializeField] public WandUI wandUIPrefab;

    private GameObject currentWandGO;
    private float wandRotationSpeed = 720f;
    private float wandCurrentAngle = 0f;
    private float angleErrorThreshold = 1f;

    private void Update()
    {
        if (currentWandGO == null) return;

        Enemy closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            Vector3 direction = closestEnemy.transform.position - towerTransform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            RotateWand(targetAngle);

            // Use angular difference calculation
            float angleDiff = Mathf.Abs(Mathf.DeltaAngle(wandCurrentAngle, targetAngle));

            if (angleDiff < angleErrorThreshold)
            {
                float distanceToEnemy = direction.magnitude;
                currentWandGO.GetComponent<Wand>().Shoot(distanceToEnemy);
            }
        }
    }

    private Enemy FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            if (!enemy) continue;

            float distance = Vector3.Distance(towerTransform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    private void RotateWand(float wandAngleDesired)
    {
        // Get shortest angular distance (-180 to 180)
        float angleDifference = Mathf.DeltaAngle(wandCurrentAngle, wandAngleDesired);

        // Choose rotation direction based on shortest path
        float rotationDirection = Mathf.Sign(angleDifference);

        // Apply rotation with speed limit
        wandCurrentAngle += rotationDirection * Mathf.Min(
            Mathf.Abs(angleDifference),
            wandRotationSpeed * Time.deltaTime
        );

        // Normalize angle to 0-360 range
        wandCurrentAngle = Mathf.Repeat(wandCurrentAngle, 360);

        // Update position and rotation (keep your existing code)
        float radians = wandCurrentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * offsetRadius;
        currentWandGO.transform.position = towerTransform.position + offset;
        currentWandGO.transform.rotation = Quaternion.Euler(0f, 0f, wandCurrentAngle);
    }

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
        currentWandGO = currentWand.gameObject;
        currentWand.towerTransform = towerTransform;
        var wandUI = Instantiate(wandUIPrefab, canvasWandInventoryContainer.transform);
        wandUI.SetWand(currentWand, GlobalData.Instance.wandSkills);
    }
}
