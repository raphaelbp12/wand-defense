using UnityEngine;

public class WandInventory : Inventory
{
    private bool isOpen = false;

    WandInventoryUI inventoryUI;

    public WandInventory(int slotAmount, GameObject panel) : base(slotAmount)
    {
        this.inventoryUI = panel.GetComponent<WandInventoryUI>();
    }

    public new ItemStack PrimaryAction(ItemStack stack, int position)
    {
        var returningStack = base.PrimaryAction(stack, position);
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

    public void CloseInventory()
    {
        inventoryUI.CloseInventory();
    }

    public void Interact()
    {
        if (!inventoryUI.IsOpen())
            inventoryUI.OpenInventory(this);
        else
            inventoryUI.CloseInventory();
    }

}