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
    private Inventory currentInventory;

    void Awake()
    {
        this.panel = this.gameObject;
        CloseInventory();
    }

    void Start()
    {
    }
    public int AddItems(ItemStack stack)
    {
        return currentInventory.AddItems(stack);
    }

    public virtual ItemStack SwapItem(Vector2 position, ItemStack stack, InventoryActionType action)
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

    public void OpenInventory(Inventory inventory)
    {
        // ...existing code...
        this.isOpen = true;
        currentInventory = inventory;
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
        // Get references to UI components
        Transform slotTransform = uiSlots[index].transform;
        Image iconImage = slotTransform.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI quantityText = slotTransform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI initialsText = slotTransform.GetChild(2).GetComponent<TextMeshProUGUI>(); // Add this to your slot prefab

        if (item != null)
        {
            SkillSO itemSO = item.GetItem();

            // Handle icon/initials
            if (itemSO.itemIcon != null)
            {
                // Show icon
                iconImage.sprite = itemSO.itemIcon;
                iconImage.enabled = true;
                initialsText.enabled = false;
            }
            else
            {
                // Show initials
                iconImage.enabled = false;
                initialsText.text = GetItemInitials(itemSO.skillName);
                initialsText.enabled = true;
            }

            // Handle quantity
            quantityText.text = itemSO.isStackable ? item.GetQuantity().ToString() : "";
        }
        else
        {
            // Clear all elements
            iconImage.sprite = null;
            iconImage.enabled = false;
            initialsText.enabled = false;
            quantityText.text = "";
        }
    }


    // Add this helper method to generate initials
    private string GetItemInitials(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return "??";

        string[] words = itemName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        System.Text.StringBuilder initials = new System.Text.StringBuilder();

        foreach (string word in words)
        {
            if (word.Length > 0 && char.IsLetter(word[0]))
            {
                initials.Append(char.ToUpper(word[0]));
            }
        }

        // Return first 3 letters if available
        return initials.Length switch
        {
            0 => "?",
            1 => itemName.Length > 1 ? $"{initials[0]}{char.ToUpper(itemName[1])}" : initials.ToString(),
            _ => initials.ToString().Substring(0, Mathf.Min(initials.Length, 3))
        };
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