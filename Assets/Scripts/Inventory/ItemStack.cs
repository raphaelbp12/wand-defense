using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ItemStack
{

    [SerializeField] private SkillSO item;
    [SerializeField] private int quantity;


    public ItemStack(SkillSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public SkillSO GetItem()
    {
        if (quantity == 0)
            return null;
        return item;
    }
    public int GetQuantity() { return quantity; }

    public int Sum(int quantity)
    {
        var amountToAdd = Mathf.Min(quantity, this.MaximumSize() - this.quantity);
        this.quantity += amountToAdd;
        return amountToAdd;
    }

    public int MaximumSize()
    {
        return this.item.stackSize;
    }

}
