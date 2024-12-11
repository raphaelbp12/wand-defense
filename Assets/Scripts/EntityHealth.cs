using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    public int maxHP;
    public int currentHP;

    // Event that gets invoked when the entity dies
    public UnityEvent OnDied;

    private void Start()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
        // TakeDamage(3);
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(currentHP - amount, 0);
        if (currentHP == 0)
        {
            // Trigger death event
            OnDied.Invoke();
        }
    }

    public bool IsAlive()
    {
        return currentHP > 0;
    }

    public void ResetHealth()
    {
        currentHP = maxHP;
    }
}
