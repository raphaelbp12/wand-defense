using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WandInventoryUI : MonoBehaviour,
    IInventoryInteraction
{
    private GameObject panel;
    private GameObject[] uiSlots;
    public GameObject uiSlotPrefab;
    bool isOpen = false;

    WandInventory currentInventory;
    const int inventorySize = 20;

    void Awake()
    {
        this.panel = this.gameObject;
        uiSlots = new GameObject[20];

        for (var i = 0; i < inventorySize; i++)
        {
            uiSlots[i] = Instantiate(uiSlotPrefab, panel.transform.GetChild(0).transform);
        }
        this.CloseInventory();
    }

    public ItemStack SwapItem(Vector2 position, ItemStack stack, InventoryActionType action)
    {
        var slotPosition = GetClosestSlot(position);
        if (slotPosition != -1)
        {
            if (action == InventoryActionType.PrimaryAction)
                return currentInventory.PrimaryAction(stack, slotPosition);
            else if (action == InventoryActionType.SecondaryAction)
                return currentInventory.SecondaryAction(stack, slotPosition);
        }

        return null;
    }

    public void OpenInventory(WandInventory inventory)
    {
        this.isOpen = true;
        currentInventory = inventory;
        RefreshSlots();
    }

    public void CloseInventory()
    {
        this.isOpen = false;
        currentInventory = null;
        this.panel.transform.GetChild(0).gameObject.SetActive(this.isOpen);
        panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
    }

    public void RefreshSlots()
    {
        if (currentInventory != null)
        {
            this.panel.transform.GetChild(0).gameObject.SetActive(this.isOpen);
            panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 65 * Mathf.Ceil((inventorySize) / 10f) + 15);
            for (var i = 0; i < inventorySize; i++)
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