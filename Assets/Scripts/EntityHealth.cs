using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    public BarController healthBarController;
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
        if (healthBarController != null)
        {
            healthBarController.SetPercentage((float)currentHP / (float)maxHP);
        }
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
