using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EntityHealth), typeof(Enemy))]
public class EnemyDeathHandler : MonoBehaviour
{
    public delegate void EnemyDied(int waveIndex);
    public event EnemyDied OnEnemyDied;
    private EntityHealth entityHealth;
    private Enemy enemy;

    private void Awake()
    {
        entityHealth = GetComponent<EntityHealth>();
        enemy = GetComponent<Enemy>();
        if (entityHealth != null)
        {
            entityHealth.OnDied.AddListener(HandleEnemyDeath);
        }
    }

    private void HandleEnemyDeath()
    {
        OnEnemyDied?.Invoke(enemy.waveIndex);
        Destroy(gameObject);
    }
}
