using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// DraggableDice is responsible for dragging the dice around
/// and deciding where it should go when dropped.
/// </summary>
public class DraggableDice : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform; 
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    
    [SerializeField] private Transform originalParent;  // The parent where the dice starts
    [SerializeField] private Vector2 originalPosition;  // The position where the dice starts
    
    // Reference to the slot the dice is currently in (if any)
    private DiceSlot currentSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Remove from current slot (if we were already in one)
        if (currentSlot != null)
        {
            currentSlot.ClearSlot();
            currentSlot = null;
        }

        // Move to the top canvas so that it can be dragged over UI
        transform.SetParent(canvas.transform, true);

        // Make the dice a bit transparent while dragging and ignore raycasts
        canvasGroup.alpha = 0.9f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update position based on pointer movement, adjusted by scaleFactor
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore full visibility and allow raycasts again
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // Perform a raycast to find if we're over a DiceSlot
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        Debug.Log("Raycast Results (OnEndDrag):");
        foreach (var result in raycastResults)
        {
            Debug.Log($"Hit: {result.gameObject.name}");
        }

        // Look for a DiceSlot in the raycast results
        DiceSlot slot = null;
        foreach (var result in raycastResults)
        {
            slot = result.gameObject.GetComponent<DiceSlot>();
            if (slot != null)
                break; // Stop at the first valid DiceSlot
        }

        // If we found a DiceSlot that is currently empty...
        if (slot != null && slot.IsEmpty)
        {
            Debug.Log("Valid empty slot found, attempting to assign dice.");
            bool assigned = slot.AssignDice(gameObject);
            if (assigned)
            {
                Debug.Log("Assignment successful.");
                currentSlot = slot; // Keep track of our new home
            }
            else
            {
                Debug.Log("Assignment failed due to restrictions, resetting to original parent.");
                ResetToOriginalParent();
            }
        }
        else
        {
            Debug.Log("No valid empty slot found, resetting to original parent.");
            ResetToOriginalParent();
        }
    }

    /// <summary>
    /// Resets the dice to its original parent and position.
    /// </summary>
    private void ResetToOriginalParent()
    {
        transform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition;
        Debug.Log($"Dice {gameObject.name} reset to original parent: {originalParent.name}");
    }
}
