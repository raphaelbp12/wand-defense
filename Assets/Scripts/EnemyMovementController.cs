using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public float speed = 2f;
    public Transform towerTransform; // The tower reference set by a spawner or in the editor

    private void Update()
    {
        if (towerTransform == null)
            return;

        // Move towards the tower
        Vector3 direction = (towerTransform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
