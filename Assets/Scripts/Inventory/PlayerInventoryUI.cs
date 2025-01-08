using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour,
    IInventoryInteraction
{
    private GameObject panel;
    private GameObject[] uiSlots;
    public GameObject uiSlotPrefab;
    bool isOpen = false;

    PlayerInventory currentInventory;
    const int inventorySize = 50;
    const int hotbarSlotAmount = 10;

    private readonly Color selectedColor = new Color(0, 0.1372f, 0.4156f, 0.8f);
    private readonly Color unselectedColor = new Color(0, 0.1372f, 0.4156f, 0.4f);

    void Awake()
    {
        this.panel = this.gameObject;
        uiSlots = new GameObject[inventorySize];

        for (var i = 0; i < hotbarSlotAmount; i++)
        {
            uiSlots[i] = Instantiate(uiSlotPrefab, panel.transform.GetChild(0).transform);
        }

        for (var i = hotbarSlotAmount; i < inventorySize; i++)
        {
            uiSlots[i] = Instantiate(uiSlotPrefab, panel.transform.GetChild(1).transform);
        }

        CloseInventory();
    }

    public ItemStack SwapItem(Vector2 position, ItemStack stack, InventoryActionType action)
    {
        var slotPosition = GetClosestSlot(position);
        if (slotPosition != -1)
        {
            if (action == InventoryActionType.PrimaryAction)
                return currentInventory.PrimaryAction(stack, slotPosition);
        }

        return null;
    }

    public void OpenInventory(PlayerInventory inventory)
    {
        if (!this.isOpen)
        {
            this.isOpen = true;
            currentInventory = inventory;
            RefreshUI();
        }
    }

    public void CloseInventory()
    {
        if (this.isOpen)
        {
            this.isOpen = false;
            RefreshUI();
        }
    }


    public void RefreshUI()
    {
        RefreshHotbar();
        RefreshSlots();
    }

    public void RefreshHotbar()
    {
        for (int i = 0; i < hotbarSlotAmount; i++)
        {
            if (i == currentInventory.GetSelectedPosition())
                uiSlots[i].GetComponent<Image>().color = selectedColor;
            else
                uiSlots[i].GetComponent<Image>().color = unselectedColor;
        }
    }


    public void RefreshSlots()
    {
        if (isOpen)
        {
            this.panel.transform.GetChild(0).gameObject.SetActive(this.isOpen);
            this.panel.transform.GetChild(1).gameObject.SetActive(this.isOpen);
            panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 65 * Mathf.Ceil((inventorySize) / 10f) + 25);
            for (var i = 0; i < inventorySize; i++)
                SetSlot(i, currentInventory.GetStack(i));
        }
        else
        {
            panel.transform.GetChild(1).gameObject.SetActive(false);
            panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70);
            for (var i = 0; i < hotbarSlotAmount; i++)
                SetSlot(i, currentInventory.GetStack(i));
        }
    }

    void SetSlot(int index, ItemStack item)
    {
        if (item != null)
        {
            uiSlots[index].transform.GetChild(0).GetComponent<Image>().sprite = item.GetItem().itemIcon;
            uiSlots[index].transform.GetChild(0).GetComponent<Image>().enabled = true;
            if (item.GetItem().isStackable)
                uiSlots[index].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.GetQuantity().ToString();
            else
                uiSlots[index].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            uiSlots[index].transform.GetChild(0).GetComponent<Image>().sprite = null;
            uiSlots[index].transform.GetChild(0).GetComponent<Image>().enabled = false;
            uiSlots[index].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public int GetClosestSlot(Vector2 position)
    {
        var slotDistances = uiSlots.Select(x => (x, Vector2.Distance(x.transform.position, position))).ToArray();
        var closestSlot = slotDistances.Aggregate((curMin, x) => x.Item2 < curMin.Item2 ? x : curMin);

        if (closestSlot.x.gameObject.activeSelf)
            return Array.IndexOf(uiSlots, closestSlot.x);
        else
            return -1;
    }

}