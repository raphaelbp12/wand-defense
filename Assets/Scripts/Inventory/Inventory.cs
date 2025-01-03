using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


public class Inventory
{
    protected Slot[] slots;

    protected int inventorySize;

    public Inventory(int slotAmount)
    {
        this.slots = new Slot[slotAmount];
        this.slots = this.slots.Select(i => new Slot()).ToArray();
        this.inventorySize = slotAmount;
    }


    public int AddItems(ItemStack stack)
    {
        int itemsAdded = 0;

        int remainingToAdd = stack.GetQuantity();
        int? slotToAdd = GetNextSlotContainingItem(stack.GetItem());
        while (remainingToAdd > 0 && slotToAdd != null)
        {
            itemsAdded += slots[slotToAdd.Value].SumStack(stack);
            remainingToAdd = stack.GetQuantity() - itemsAdded;
            slotToAdd = GetNextSlotContainingItem(stack.GetItem(), slotToAdd.Value + 1);
        }

        int? emptySlot = GetNextEmptySlot();
        if (emptySlot != null && remainingToAdd > 0)
        {
            slots[emptySlot.Value].SetSlot(new ItemStack(stack.GetItem(), remainingToAdd));
            itemsAdded += remainingToAdd;
        }

        return itemsAdded;
    }

    public int AddItems(ItemStack stack, int position)
    {
        if (position >= inventorySize) return 0;

        var slot = slots[position];
        if (slot.IsEmpty())
            slot.SetSlot(stack);
        else if (slot.GetStack().GetItem() == stack.GetItem())
            return slot.SumStack(stack);

        return 0;
    }


    public int? GetNextEmptySlot(int from = 0)
    {
        int emptySlot = -1;
        for (var i = 0; i < inventorySize; i++)
        {
            if (slots[i].IsEmpty())
            {
                emptySlot = i;
                break;
            }
        }

        if (emptySlot >= 0)
            return emptySlot;
        else
            return null;
    }

    public int? GetNextSlotContainingItem(SkillSO item, int from = 0)
    {
        int contaningSlot = -1;
        for (var i = from; i < inventorySize; i++)
        {
            if (!slots[i].IsEmpty() && slots[i].GetStack().GetItem() == item)
            {
                contaningSlot = i;
                break;
            }
        }
        if (contaningSlot >= 0)
            return contaningSlot;
        else
            return null;
    }

    public ItemStack RemoveItems(int quantity, int position)
    {
        if (position >= inventorySize) return null;

        var slot = slots[position];

        if (slot.GetQuantity() <= quantity)
        {
            slot.Clear();
            return slot.GetStack();
        }
        else
        {

            slot.AddQuantity(-quantity);
            return new ItemStack(slot.GetItem(), quantity);
        }
    }

    public ItemStack RemoveStack(int position)
    {
        if (position >= inventorySize) return null;
        var stack = GetStack(position);
        slots[position].Clear();
        return stack;
    }

    public List<ItemStack> GetAllStack()
    {
        return slots.Select(x => x.GetStack()).ToList();
    }

    public ItemStack GetStack(int position)
    {
        if (position >= inventorySize) return null;

        return slots[position].GetStack();
    }

    public ItemStack PrimaryAction(ItemStack stack, int position)
    {
        ItemStack returningStack = null;
        // no item received
        if (stack == null || stack.GetItem() == null)
        {
            returningStack = this.RemoveStack(position);
        }
        // item received / no item in the position
        else if (GetStack(position) == null)
        {
            this.AddItems(stack, position);
            returningStack = null;
        }
        // item received / different item in position
        else if (GetStack(position).GetItem() != stack.GetItem())
        {
            var removedStack = this.RemoveStack(position);
            this.AddItems(stack, position);
            returningStack = removedStack;
        }
        // item received / equal item in position
        else
        {
            int addedItems = AddItems(stack, position);
            if (addedItems == stack.GetQuantity())
                returningStack = null;
            else
                returningStack = new ItemStack(stack.GetItem(), stack.GetQuantity() - addedItems);
        }
        return returningStack;
    }

    public ItemStack SecondaryAction(ItemStack stack, int position)
    {
        ItemStack returningStack = null;
        // no item received
        if (stack == null || stack.GetItem() == null)
        {
            var selectedStack = this.GetStack(position);
            if (selectedStack != null)
                returningStack = this.RemoveItems(selectedStack.GetQuantity() / 2, position);
        }
        // item received / no item in the position
        else if (GetStack(position) == null)
        {
            this.AddItems(new ItemStack(stack.GetItem(), 1), position);
            if (stack.GetQuantity() > 1)
                returningStack = new ItemStack(stack.GetItem(), stack.GetQuantity() - 1);
            else
                returningStack = null;
        }
        // item received / different item in position
        else if (GetStack(position).GetItem() != stack.GetItem())
        {
            returningStack = stack;
        }
        // item received / equal item in position
        else
        {
            int addedItems = this.AddItems(new ItemStack(stack.GetItem(), 1), position);
            if (addedItems == stack.GetQuantity())
                returningStack = null;
            else
                returningStack = new ItemStack(stack.GetItem(), stack.GetQuantity() - addedItems);
        }
        return returningStack;
    }

}