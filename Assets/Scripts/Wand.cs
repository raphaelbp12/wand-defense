using UnityEngine;

public class Wand : MonoBehaviour
{
    [HideInInspector]
    public Transform towerTransform;
    public float attackInterval = 2f;
    public float attackRange = 10f;
    public int projectileDamage = 10;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;

    private float attackTimer = 0f;

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            // Attempt to find and attack the closest enemy
            Enemy closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
                if (distance <= attackRange)
                {
                    ShootProjectileAt(closestEnemy);
                }
            }

            attackTimer = 0f;
        }
    }

    private Enemy FindClosestEnemy()
    {
        // Using the new API:
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            if (!enemy) continue;

            float distance = Vector3.Distance(towerTransform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }


    private void ShootProjectileAt(Enemy target)
    {
        if (projectilePrefab == null || target == null) return;

        GameObject projObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(target.transform, projectileDamage, projectileSpeed);
        }
    }
}
