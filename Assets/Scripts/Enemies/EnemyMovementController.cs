using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    private Enemy enemy;
    public float speed = 2f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (enemy.towerTransform == null) return;

        // Move towards the tower
        Vector3 direction = (enemy.towerTransform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
