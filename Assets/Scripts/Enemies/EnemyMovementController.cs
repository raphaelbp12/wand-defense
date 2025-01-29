using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    private Enemy enemy;
    private Rigidbody2D rb;
    public float speed = 2f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>(); // Add Rigidbody2D to your enemy prefab
    }

    private void Update()
    {
        if (enemy.towerTransform == null) return;

        Vector3 direction = (enemy.towerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * speed; // Use velocity for physics-aware movement
    }
}