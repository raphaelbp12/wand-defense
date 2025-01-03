using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private Vector3 direction;
    private float damage;
    private float speed;

    public float maxLifetime = 5f;
    private float lifetimeTimer = 0f;

    // Ensure the projectile's collider is set to "isTrigger = true"
    // and that collision layers allow the projectile to detect enemies.

    public void Initialize(Vector3 direction, float damage, float speed)
    {
        this.direction = direction.normalized;
        this.damage = damage;
        this.speed = speed;
    }

    private void Update()
    {
        lifetimeTimer += Time.deltaTime;

        // Destroy if lifetime exceeded
        if (lifetimeTimer >= maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        // Move in the given direction
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if we've hit an enemy
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Deal damage to the enemy
            EntityHealth health = enemy.GetComponent<EntityHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // Destroy the projectile after hitting an enemy
            Destroy(gameObject);
        }
    }
}
