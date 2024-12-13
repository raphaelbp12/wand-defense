using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private int damage;
    private float speed;

    // The maximum time (in seconds) the projectile will live before self-destructing.
    public float maxLifetime = 5f;
    private float lifetimeTimer = 0f;

    public void Initialize(Transform target, int damage, float speed)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
    }

    private void Update()
    {
        lifetimeTimer += Time.deltaTime;

        // If the projectile has exceeded its lifetime, destroy it.
        if (lifetimeTimer >= maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        // If the target is lost, just move forward or self-destruct.
        // For simplicity, if we lose the target, we can just destroy the projectile
        // or in an advanced scenario, keep going straight. For now, let's destroy it.
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Check if close enough to hit the target
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 0.5f)
        {
            EntityHealth health = target.GetComponent<EntityHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // Destroy the projectile upon hitting the target
            Destroy(gameObject);
        }
    }
}
