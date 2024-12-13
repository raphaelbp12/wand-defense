using UnityEngine;

[RequireComponent(typeof(EnemyMovementController))]
public class EnemyAttack : MonoBehaviour
{
    private Enemy enemy;
    public float attackInterval = 1f; // how often the enemy attacks in seconds
    public int baseDamage = 10;
    public float attackRange = 1.5f;

    private float attackTimer;
    private EntityHealth towerHealth;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        // Get a reference to the tower’s health from the assigned tower transform
        if (enemy != null && enemy.towerTransform != null)
        {
            towerHealth = enemy.towerTransform.GetComponent<EntityHealth>();
        }
    }

    private void Update()
    {
        if (towerHealth == null) return;

        // Check distance to tower
        float distance = Vector3.Distance(transform.position, enemy.towerTransform.position);
        if (distance <= attackRange)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                PerformAttack();
                attackTimer = 0f;
            }
        }
        else
        {
            // Enemy not in range to attack, reset timer or let it count up again
            // (Keeping it counting or resetting is a design choice)
        }
    }

    private void PerformAttack()
    {
        if (towerHealth.IsAlive())
        {
            towerHealth.TakeDamage(baseDamage);
        }
    }
}
