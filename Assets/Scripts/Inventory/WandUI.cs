using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WandUI : InventoryUI
{
    private Wand wand;
    private Inventory wandInventory;
    private bool shouldSyncInv = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitInventory();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void InitInventory()
    {
        shouldSyncInv = true;
    }

    public void SetWand(Wand wand, List<SkillSO> savedState)
    {
        this.wand = wand;
        wandInventory = new Inventory(10);
        PopulateInventory(savedState.Count() > 0 ? savedState : wand.initialSkills);
        OpenInventory(wandInventory);
        RecalculateStats();
        this.wand.Initialize();
    }

    public void PopulateInventory(List<SkillSO> skills)
    {
        foreach (SkillSO skill in skills)
        {
            var itemStack = new ItemStack(skill, 1);
            var slot = wandInventory.AddItems(itemStack);

            if (slot == -1)
            {
                shouldSyncInv = true;
                return;
            }
        }

    }

    private void RecalculateStats()
    {
        var allItemStacks = wandInventory.GetAllStacks();
        var allSkills = allItemStacks.Where(x => x != null).Select(x => x.GetItem()).ToList();

        wand.RecalculateStats(allSkills);
    }

    public override ItemStack SwapItem(Vector2 position, ItemStack stack, InventoryActionType action)
    {
        // 1) Do something before
        Debug.Log($"[WandUI] About to SwapItem. Additional step");

        // 2) Now call the base InventoryUI SwapItem
        ItemStack result = base.SwapItem(position, stack, action);

        RecalculateStats();
        // 3) Optionally do something after
        Debug.Log("[WandUI] SwapItem completed.");
        GlobalData.Instance.SaveWandSkills(wandInventory.GetAllStacks().Where(x => x != null).Select(x => x.GetItem()).ToList());
        return result;
    }
}
