using UnityEngine;

public enum InventoryActionType
{
    PrimaryAction
}

public interface IInventoryInteraction : IMouseIntercept
{
    InterceptionType IMouseIntercept.GetInterceptionType() { return InterceptionType.ItemSwap; }

    public ItemStack SwapItem(Vector2 position, ItemStack stack, InventoryActionType action);
}