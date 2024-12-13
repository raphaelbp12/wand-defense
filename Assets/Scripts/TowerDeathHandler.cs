using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EntityHealth))]
public class TowerDeathHandler : MonoBehaviour
{
    public UnityEvent OnTowerDied; // Invoked when the tower is destroyed

    private EntityHealth entityHealth;

    private void Awake()
    {
        entityHealth = GetComponent<EntityHealth>();
        if (entityHealth != null)
        {
            entityHealth.OnDied.AddListener(HandleTowerDeath);
        }
    }

    private void HandleTowerDeath()
    {
        Debug.Log("Tower destroyed. Triggering OnTowerDied event...");
        OnTowerDied.Invoke();
    }
}
