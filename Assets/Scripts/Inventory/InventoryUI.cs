using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IInventoryInteraction
{
    private GameObject panel;
    private GameObject[] uiSlots;
    public GameObject uiSlotPrefab;
    bool isOpen = false;

    // Use the unified Inventory class
    Inventory currentInventory;

    void Awake()
    {
        this.panel = this.gameObject;
        CloseInventory();
    }

    void Start()
    {
        var inventory = new Inventory(50);
        this.currentInventory = inventory;
        this.OpenInventory();
    }

    public ItemStack SwapItem(Vector2 position, ItemStack stack, InventoryActionType action)
    {
        if (currentInventory == null)
            return stack;
        // ...existing code...
        var slotPosition = GetClosestSlot(position);
        if (slotPosition != -1 && action == InventoryActionType.PrimaryAction)
        {
            var returningStack = currentInventory.PrimaryAction(stack, slotPosition);
            RefreshSlots();
            return returningStack;
        }
        return null;
    }

    public void OpenInventory()
    {
        // ...existing code...
        this.isOpen = true;
        EnsureSlots(currentInventory.GetSize());
        RefreshSlots();
    }

    public void CloseInventory()
    {
        // ...existing code...
        this.isOpen = false;
        currentInventory = null;
        panel.transform.GetChild(0).gameObject.SetActive(isOpen);
        // ...existing code...
    }

    void RefreshSlots()
    {
        // ...existing code...
        if (currentInventory != null)
        {
            var inventorySize = currentInventory.GetSize();

            panel.transform.GetChild(0).gameObject.SetActive(isOpen);
            panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 65 * Mathf.Ceil(inventorySize / 10f) + 25);
            for (var i = 0; i < inventorySize; i++)
            {
                SetSlot(i, currentInventory.GetStack(i));
            }
        }
    }

    void EnsureSlots(int desiredCount)
    {
        if (uiSlots != null && uiSlots.Length == desiredCount)
            return;

        var currentCount = uiSlots != null ? uiSlots.Length : 0;
        if (desiredCount < currentCount)
        {
            for (int i = currentCount - 1; i >= desiredCount; i--)
            {
                Destroy(uiSlots[i]);
            }
        }

        GameObject[] newSlots = new GameObject[desiredCount];
        for (int i = 0; i < desiredCount; i++)
        {
            if (i < currentCount)
                newSlots[i] = uiSlots[i];
            else
                newSlots[i] = Instantiate(uiSlotPrefab, panel.transform.GetChild(0).transform);
        }
        uiSlots = newSlots;
    }

    void SetSlot(int index, ItemStack item)
    {
        // ...existing code...
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
        // ...existing code...
        return isOpen;
    }

    public int GetClosestSlot(Vector2 position)
    {
        // ...existing code...
        var slotDistances = uiSlots.Select(x => (x, Vector2.Distance(x.transform.position, position))).ToArray();
        var closestSlot = slotDistances.Aggregate((curMin, x) => x.Item2 < curMin.Item2 ? x : curMin);

        if (closestSlot.x.gameObject.activeSelf)
            return Array.IndexOf(uiSlots, closestSlot.x);
        else
            return -1;
    }
}