using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;

    // Single-run data
    public int[] waveEnemiesRemaining;
    public int currentWaveIndex { get; private set; }
    public int playerGold;

    // Example meta progress data
    // (in practice, you might store more complex data and encode it)
    public int totalCrystals;
    public bool isNewWandUnlocked;
    public List<SkillSO> wandSkills { get; private set; } = new List<SkillSO>();
    public Inventory PlayerInventory { get; private set; }

    public TowerData TowerData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMetaProgress();
            TowerData = new TowerData(10);
            PlayerInventory = new Inventory(10);
            ResetSingleRunData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Single-run reset
    public void ResetSingleRunData()
    {
        TowerData.Reset();
        waveEnemiesRemaining = null;
        currentWaveIndex = 1;
    }

    public int IncrementCurrentWaveIndex()
    {
        currentWaveIndex++;
        return currentWaveIndex;
    }

    public void SaveWandSkills(List<SkillSO> skills)
    {
        wandSkills = skills;
    }

    // --- METAPROGRESS METHODS ---

    // Call this when you need to save meta data (e.g., after a run)
    public void SaveMetaProgress()
    {
        PlayerPrefs.SetInt("PlayerGold", playerGold);
        PlayerPrefs.SetInt("TotalCrystals", totalCrystals);
        PlayerPrefs.SetInt("IsNewWandUnlocked", isNewWandUnlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Load meta data at startup
    public void LoadMetaProgress()
    {
        playerGold = PlayerPrefs.GetInt("PlayerGold", 0);
        totalCrystals = PlayerPrefs.GetInt("TotalCrystals", 0);
        isNewWandUnlocked = PlayerPrefs.GetInt("IsNewWandUnlocked", 0) == 1;
    }
}
