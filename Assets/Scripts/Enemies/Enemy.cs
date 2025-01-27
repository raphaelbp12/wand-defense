using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(EnemyMovementController), typeof(EntityHealth))]
public class Enemy : MonoBehaviour
{
    private EntityHealth entityHealth;
    private EnemyMovementController movementController;

    public Transform towerTransform { get; private set; }
    public int waveIndex { get; private set; }

    public void InitializeEnemy(EnemyTypeDefinition def, int wave, Transform tower)
    {
        waveIndex = wave;
        towerTransform = tower;

        if (entityHealth == null) entityHealth = GetComponent<EntityHealth>();
        if (movementController == null) movementController = GetComponent<EnemyMovementController>();

        // Calculate HP
        float computedHP = def.baseHP + def.hpGrowthPerWave * wave;
        computedHP = Mathf.Min(computedHP, def.maxHP);

        entityHealth.SetMaxHP(computedHP);
        entityHealth.ResetHealth();

        // Calculate Speed
        float computedSpeed = def.baseSpeed + def.speedGrowthPerWave * wave;
        computedSpeed = Mathf.Min(computedSpeed, def.maxSpeed);

        movementController.speed = computedSpeed;
    }

    private void Awake()
    {
        entityHealth = GetComponent<EntityHealth>();
        movementController = GetComponent<EnemyMovementController>();
    }
}
