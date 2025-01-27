using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Slot
{

    private ItemStack stack;

    public Slot(ItemStack stack)
    {
        this.stack = stack;
    }

    public Slot()
    {
        this.stack = null;
    }

    public Slot(Slot slot)
    {
        this.SetSlot(slot.GetStack());
    }

    public void SetSlot(ItemStack stack)
    {
        this.stack = stack;
    }

    public int SumStack(ItemStack stack)
    {
        if (this.stack.GetItem() == stack.GetItem())
            return this.stack.Sum(stack.GetQuantity());
        else
            return 0;
    }

    public int AddQuantity(int quantity)
    {
        return this.stack.Sum(quantity);
    }

    public int SubQuantity(int quantity)
    {
        return this.stack.Sum(-quantity);
    }

    public ItemStack GetStack() { return stack; }

    public void Clear()
    {
        this.stack = null;
    }

    public bool IsEmpty()
    {
        return (stack == null);
    }

    public SkillSO GetItem()
    {
        if (stack == null)
            return null;
        else
            return this.stack.GetItem();
    }

    public int? GetQuantity()
    {
        if (stack == null)
            return null;
        else
            return this.stack.GetQuantity();
    }


}