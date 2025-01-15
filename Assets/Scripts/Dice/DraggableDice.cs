using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableDice : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform; 
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    
    [SerializeField] private Transform originalParent;  // The parent where the dice starts
    [SerializeField] private Vector2 originalPosition;  // The position where the dice starts
    
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
        if (currentSlot != null)
        {
            currentSlot.ClearSlot();
            currentSlot = null;
        }

        // Move to the top canvas so that it can be dragged over UI
        transform.SetParent(canvas.transform, true);

        canvasGroup.alpha = 0.9f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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

        DiceSlot slot = null;
        foreach (var result in raycastResults)
        {
            slot = result.gameObject.GetComponent<DiceSlot>();
            if (slot != null)
                break;
        }

        if (slot != null && slot.IsEmpty)
        {
            Debug.Log("Valid empty slot found, attempting to assign dice.");
            bool assigned = slot.AssignDice(gameObject);
            if (assigned)
            {
                Debug.Log("Assignment successful.");
                currentSlot = slot;
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

    public void ResetToOriginalParent()
    {
        transform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition;
        Debug.Log($"Dice {gameObject.name} reset to original parent: {originalParent.name}");
    }
}
