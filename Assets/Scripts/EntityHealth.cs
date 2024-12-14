using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    public BarController healthBarController;
    public GameObject damageTextPrefab;
    public RectTransform tooltipSpawner;
    private Canvas canvas;
    public int maxHP;
    public int currentHP;

    // Event that gets invoked when the entity dies
    public UnityEvent OnDied;

    private void Start()
    {
        currentHP = maxHP;
        canvas = GetComponentInChildren<Canvas>();
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
        ShowDamageText(amount, gameObject.tag == "Player" ? Color.red : Color.white);
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


    private void ShowDamageText(int damage, Color color)
    {
        // Instantiate the damage text prefab at the enemy's position
        GameObject damageTextObject = Instantiate(damageTextPrefab, tooltipSpawner.localPosition, Quaternion.identity);


        // Set the damage value on the instantiated damage text
        DamageText damageText = damageTextObject.GetComponent<DamageText>();
        if (damageText != null)
        {
            damageTextObject.transform.SetParent(canvas.transform, false);

            damageText.GetComponent<TMPro.TMP_Text>().text = damage.ToString();
            if (color != null)
            {
                damageText.GetComponent<TMPro.TMP_Text>().color = color;
            }
        }
    }
}
