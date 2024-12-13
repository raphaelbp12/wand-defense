using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private int damage;
    private float speed;

    public void Initialize(Transform target, int damage, float speed)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // If close enough, deal damage and destroy the projectile
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 0.5f)
        {
            EntityHealth health = target.GetComponent<EntityHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
