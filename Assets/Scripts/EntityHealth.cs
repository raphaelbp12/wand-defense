using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    public BarController healthBarController;
    public GameObject damageTextPrefab;
    public RectTransform tooltipSpawner;
    private Canvas canvas;
    [HideInInspector] public float maxHP;
    [HideInInspector] public float currentHP { get; private set; }

    // Event that gets invoked when the entity dies
    public UnityEvent OnDied;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        if (currentHP == 0)
        {
            currentHP = maxHP;
        }
    }

    private void Update()
    {
        if (healthBarController != null && maxHP > 0)
        {
            healthBarController.SetPercentage(currentHP / maxHP);
            healthBarController.SetText(currentHP + "/" + maxHP);
        }
    }

    public void SetMaxHP(float amount)
    {
        maxHP = amount;
    }

    public void SetCurrentHP(float amount)
    {
        currentHP = amount;
    }

    public void TakeDamage(float amount)
    {
        ShowDamageText(amount, gameObject.tag == "Player" ? Color.red : Color.white);
        currentHP = Mathf.Max(currentHP - amount, 0f);
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


    private void ShowDamageText(float damage, Color color)
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
