using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(EnemyMovementController), typeof(EntityHealth))]
public class Enemy : MonoBehaviour
{
    private EntityHealth entityHealth;
    private EnemyMovementController movementController;
    public Transform towerTransform { get; private set; }
    public int waveIndex { get; private set; }

    private void Awake()
    {
        // Get references to attached components
        entityHealth = GetComponent<EntityHealth>();
        movementController = GetComponent<EnemyMovementController>();
    }

    private void Update()
    {
        // For now, the Enemy doesn't do much here.
        // Movement and health are handled by their respective components.
        // Additional enemy-specific logic (e.g., attacking) could go here later.
    }

    public void Initialize(Transform towerTransform, int waveIndex)
    {
        this.towerTransform = towerTransform;
        this.waveIndex = waveIndex;
    }
}
