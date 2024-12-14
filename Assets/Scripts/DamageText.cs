using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float floatSpeed = 0.1f;
    public float floatDuration = 0.5f;

    void Start()
    {
        // Automatically destroy the damage text after a certain duration
        Destroy(gameObject, floatDuration);
    }

    void Update()
    {
        // Float the damage text upward
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
    }
}
