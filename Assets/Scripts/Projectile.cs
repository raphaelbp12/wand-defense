using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private Vector3 direction;
    private float lifetimeTimer = 0f;
    private ProjectileData projectileData;

    public void Initialize(Vector3 direction, ProjectileData data)
    {
        this.direction = direction.normalized;
        this.projectileData = data;
    }

    private void Update()
    {
        lifetimeTimer += Time.deltaTime;

        var maxDuration = projectileData.statTable.GetStat(StatType.Duration).value;
        // Destroy if lifetime exceeded
        if (lifetimeTimer >= maxDuration)
        {
            Destroy(gameObject);
            return;
        }

        var speed = projectileData.statTable.GetStat(StatType.TravelSpeed).value;
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
                var damage = projectileData.statTable.GetStat(StatType.Damage).value;
                health.TakeDamage(damage);
            }

            // Destroy the projectile after hitting an enemy
            Destroy(gameObject);
        }
    }
}
