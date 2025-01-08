using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;



public class InteractionController : MonoBehaviour
{
    private Camera mainCam;

    [SerializeField] private GameObject stackCursor;
    private ItemStack movingStack = null;

    [SerializeField] public InventoryUI inventoryPanel;
    private Inventory inventory;


    private bool wasInit = false;

    void Start()
    {
        if (stackCursor != null && inventoryPanel != null)
        {
            Init(inventoryPanel);
        }
    }


    public void Init(InventoryUI panel)
    {
        if (wasInit) return;

        mainCam = Camera.main;
        inventoryPanel = panel;
        inventory = new Inventory(50);
        inventoryPanel.OpenInventory(inventory);
    }

    private void Update()
    {
        RefreshCursor();

        IsMouseOverComponent<EntityHealth>(1, 1);

        if (Input.GetKeyDown("e"))
        {
            if (inventoryPanel.IsOpen())
                inventoryPanel.CloseInventory();
            else
                inventoryPanel.OpenInventory(inventory);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var uiElems = GetUIUnderMouse();
            if (uiElems.Count > 0)
            {
                foreach (var uiElem in uiElems)
                {
                    switch (uiElem.GetInterceptionType())
                    {
                        case (InterceptionType.ItemSwap):
                            movingStack = ((IInventoryInteraction)uiElem).SwapItem(Input.mousePosition, movingStack, InventoryActionType.PrimaryAction);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    void RefreshCursor()
    {
        if (movingStack != null && movingStack.GetItem() != null)
        {
            stackCursor.SetActive(true);
            stackCursor.transform.GetComponentInChildren<UnityEngine.UI.Image>().sprite = movingStack.GetItem().itemIcon;
            if (movingStack.GetItem().isStackable)
                stackCursor.transform.GetComponentInChildren<TextMeshProUGUI>().text = movingStack.GetQuantity().ToString();
            else
                stackCursor.transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
            stackCursor.transform.position = Input.mousePosition;
        }
        else
        {
            stackCursor.SetActive(false);
        }
    }

    private bool IsMouseOverComponent<T>(float boxWidth, float boxHeight) where T : Component
    {
        boxWidth = 0.90f * boxWidth;
        boxHeight = 0.90f * boxHeight;
        // Convert mouse position to world position for 2D OverlapBox
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Align the mouse position to the nearest whole numbers
        var worldX = Mathf.FloorToInt(mouseWorldPosition.x) + 0.5f; // Added 0.5 to center the box
        var worldY = Mathf.FloorToInt(mouseWorldPosition.y) + 0.5f; // Added 0.5 to center the box

        // Define the size of the box
        Vector2 boxSize = new Vector2(boxWidth, boxHeight);

        // Get all colliders that are within the specified box area around the mouse position
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(new Vector2(worldX, worldY), boxSize, 0f);

        // Check each collider in the array for the component T
        foreach (var hitCollider in hitColliders)
        {
            T component = hitCollider.GetComponent<T>();

            // If a collider with the component is found, return true
            if (component != null)
            {
                // Drawing the debug box before returning true for visualization
                DrawDebugBox(new Vector2(worldX, worldY), boxSize, Color.red, 2f);
                return true;
            }
        }

        // Drawing the debug box in red if no component found, for visualization
        DrawDebugBox(new Vector2(worldX, worldY), boxSize, Color.green, 2f);

        // If no object with the component was found, return false
        return false;
    }

    private void DrawDebugBox(Vector2 position, Vector2 size, Color color, float duration)
    {
        // Calculate half size for drawing
        Vector2 halfSize = size * 0.5f;

        // Calculate corners
        Vector2 topLeft = position + new Vector2(-halfSize.x, halfSize.y);
        Vector2 topRight = position + new Vector2(halfSize.x, halfSize.y);
        Vector2 bottomRight = position + new Vector2(halfSize.x, -halfSize.y);
        Vector2 bottomLeft = position + new Vector2(-halfSize.x, -halfSize.y);

        // Draw lines between the corners of the box
        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }

    private List<IMouseIntercept> GetUIUnderMouse()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        List<IMouseIntercept> gameObjects = new List<IMouseIntercept>();

        for (int i = 0; i < raycastResults.Count; i++)
        {
            var interceptComponent = raycastResults[i].gameObject.GetComponent<IMouseIntercept>();
            if (interceptComponent != null)
            {
                gameObjects.Add(interceptComponent);
            }
        }

        return gameObjects;
    }
}