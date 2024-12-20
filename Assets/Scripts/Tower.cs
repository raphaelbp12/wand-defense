using UnityEngine;

[RequireComponent(typeof(EntityHealth))]
public class Tower : MonoBehaviour
{
    private EntityHealth entityHealth;

    private void Awake()
    {
        // Get references to attached components
        entityHealth = GetComponent<EntityHealth>();
    }

    private void Update()
    {
        // For now, the Enemy doesn't do much here.
        // Movement and health are handled by their respective components.
        // Additional enemy-specific logic (e.g., attacking) could go here later.
    }
}
