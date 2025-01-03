using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInventory : Inventory
{
    private int hotbarSlotAmount = 10;
    private int hotbarSelection = 0;

    PlayerInventoryUI inventoryUI;

    public PlayerInventory(int slotAmount, PlayerInventoryUI panel) : base(slotAmount)
    {
        this.inventoryUI = panel.GetComponent<PlayerInventoryUI>();
        inventoryUI.OpenInventory(this);
        this.AddItems(new ItemStack(ItemAtlas.instance.increaseDamageLvl1, 1));
        this.AddItems(new ItemStack(ItemAtlas.instance.increaseProjectileSpeedLvl1, 1));
        inventoryUI.CloseInventory();
    }

    public new ItemStack PrimaryAction(ItemStack stack, int position)
    {
        var returningStack = base.PrimaryAction(stack, position);
        inventoryUI.RefreshSlots();
        return returningStack;
    }

    public new ItemStack SecondaryAction(ItemStack stack, int position)
    {
        var returningStack = base.SecondaryAction(stack, position);
        inventoryUI.RefreshSlots();
        return returningStack;
    }

    public new int AddItems(ItemStack stack)
    {
        var result = base.AddItems(stack);
        inventoryUI.RefreshSlots();
        return result;
    }

    public new int AddItems(ItemStack stack, int position)
    {
        var result = base.AddItems(stack, position);
        inventoryUI.RefreshSlots();
        return result;
    }

    public new ItemStack RemoveItems(int quantity, int position)
    {
        var result = base.RemoveItems(quantity, position);
        inventoryUI.RefreshSlots();
        return result;
    }

    public new ItemStack RemoveStack(int position)
    {
        var result = base.RemoveStack(position);
        inventoryUI.RefreshSlots();
        return result;
    }


    public void SelectNextSlot()
    {
        var a = hotbarSlotAmount + hotbarSelection + 1;
        hotbarSelection = a % hotbarSlotAmount;
        inventoryUI.RefreshHotbar();
    }

    public void SelectPreviousSlot()
    {
        var a = hotbarSlotAmount + hotbarSelection - 1;
        hotbarSelection = a % hotbarSlotAmount;
        inventoryUI.RefreshHotbar();
    }

    public void SelectSlot(int newSelection)
    {
        if (newSelection >= 0 && newSelection < hotbarSlotAmount)
        {
            hotbarSelection = newSelection;
            inventoryUI.RefreshHotbar();
        }
    }

    public ItemStack GetSelectedSlot()
    {
        return slots[hotbarSelection].GetStack();
    }

    public int GetSelectedPosition()
    {
        return hotbarSelection;
    }


    public void Interact()
    {
        if (!inventoryUI.IsOpen())
            inventoryUI.OpenInventory(this);
        else
            inventoryUI.CloseInventory();
    }

}