using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    [SerializeField] public InventoryUI inventoryUIPrefab;
    [SerializeField] public GameObject canvasPlayerInventoryContainer;

    public InventoryUI PlayerInventoryUIInstance;

    [Header("Upgrades")]
    public float baseCoinDropChance = 0.1f;  // Base 10% chance
    public int coinDropChanceUpgradeCost = 50;

    // Future upgrades can be added here
    // public int damageUpgradeCost = 100;
    // public float damageMultiplier = 1.0f; // increments when upgraded

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitInventory();
        }
        else
            Destroy(this);
    }

    public void InitInventory()
    {
        var playerInventory = GlobalData.Instance.PlayerInventory;
        var playerInventoryUI = Instantiate(inventoryUIPrefab, canvasPlayerInventoryContainer.transform);
        PlayerInventoryUIInstance = playerInventoryUI.GetComponent<InventoryUI>();
        playerInventoryUI.OpenInventory(playerInventory);
    }

    public float GetCoinDropChance()
    {
        // If multiple upgrades modify coin drop chance, compute final value here
        return baseCoinDropChance;
    }

    public void BuyCoinDropChanceUpgrade()
    {
        if (CurrencyManager.Instance.TrySpendGold(coinDropChanceUpgradeCost))
        {
            // Increase coin drop chance by some increment
            baseCoinDropChance += 0.05f; // for example, now 15% chance
            Debug.Log("Coin drop chance upgraded. New chance: " + baseCoinDropChance);
            // Update UI or notify player
        }
        else
        {
            Debug.Log("Not enough gold to buy coin drop chance upgrade.");
        }
    }

    // Placeholder for other upgrades
    // public void BuyDamageUpgrade()
    // {
    //     if (CurrencyManager.Instance.TrySpendGold(damageUpgradeCost))
    //     {
    //         damageMultiplier += 0.2f;
    //         Debug.Log("Damage upgraded. New multiplier: " + damageMultiplier);
    //     }
    //     else
    //     {
    //         Debug.Log("Not enough gold to buy damage upgrade.");
    //     }
    // }
}
