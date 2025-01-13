using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public TMPro.TMP_Text goldText;

    private UpgradeManager upgradeManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if (goldText != null)
        {
            goldText.text = GlobalData.Instance.playerGold.ToString();
        }
    }

    private void Start()
    {
        // Find the UpgradeManager (if in the same scene)
        upgradeManager = UpgradeManager.Instance;

        // Subscribe to enemy death events if necessary
        // Assuming EnemySpawner or WaveManager subscribes enemies to OnEnemyDied
        // Or we could do a global event system:
        // EnemyDeathHandler.OnAnyEnemyDied += HandleEnemyDied;
    }

    // This should be called by the EnemyDeathHandler when any enemy dies
    public void OnEnemyDied()
    {
        // Get coin drop chance from UpgradeManager parameters
        float coinDropChance = upgradeManager != null ? upgradeManager.GetCoinDropChance() : 0.1f;

        // Roll the dice
        if (Random.value <= coinDropChance)
        {
            int coinAmount = 1; // Could also be influenced by upgrades
            AddGold(coinAmount);
        }
    }

    public void AddGold(int amount)
    {
        GlobalData.Instance.playerGold += amount;
        Debug.Log("Gold added. Current gold: " + GlobalData.Instance.playerGold);
        // Update UI if needed
    }

    public bool TrySpendGold(int amount)
    {
        if (GlobalData.Instance.playerGold >= amount)
        {
            GlobalData.Instance.playerGold -= amount;
            Debug.Log("Spent " + amount + " gold. Remaining: " + GlobalData.Instance.playerGold);
            // Update UI if needed
            return true;
        }

        Debug.Log("Not enough gold to spend. Current gold: " + GlobalData.Instance.playerGold + ", required: " + amount);
        return false;
    }
}
