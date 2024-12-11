using UnityEngine;

[RequireComponent(typeof(EntityHealth))]
public class TowerDeathHandler : MonoBehaviour
{
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
        // Run logic to restart the game
        // For example, you might call GameManager.Instance.EndGame();
        // or show a "Game Over" UI panel. For now, we’ll just log it.
        Debug.Log("Tower destroyed. Restarting the game...");

        // Example: Access GameManager to handle restart
        // GameManager.Instance.RestartGame();
    }
}
