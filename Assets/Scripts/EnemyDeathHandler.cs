using UnityEngine;
using UnityEngine.Events;

public class EnemyDeathHandler : MonoBehaviour
{
    public delegate void EnemyDied();
    public event EnemyDied OnEnemyDied;

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
        OnEnemyDied?.Invoke();
        Destroy(gameObject);
    }
}
