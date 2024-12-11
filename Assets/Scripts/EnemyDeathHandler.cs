using UnityEngine;

[RequireComponent(typeof(EntityHealth))]
public class EnemyDeathHandler : MonoBehaviour
{
    private EntityHealth entityHealth;

    private void Awake()
    {
        entityHealth = GetComponent<EntityHealth>();
        if (entityHealth != null)
        {
            entityHealth.OnDied.AddListener(HandleEnemyDeath);
        }
    }

    private void HandleEnemyDeath()
    {
        // Perform any enemy-specific death actions (e.g., spawn loot, increase score)
        // Then destroy the enemy object
        Destroy(gameObject);
    }
}
