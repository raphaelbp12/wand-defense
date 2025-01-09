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

    void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        RefreshCursor();

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