using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Slot Restriction")]
    [Tooltip("Assign a DiceSlotRestrictionSO here if this slot is restricted.")]
    public DiceSlotRestrictionSO slotRestriction;

    [Header("Requirement Visuals")]
    public Image requirementsImage;   // Image showing requirements
    public Image fulfilledImage;      // Image to show when fulfilled

    private GameObject currentDice;
    public bool isRequirementFulfilled = false; // Tracks if requirements are fulfilled

    public bool IsEmpty => currentDice == null;

    private void Awake()
    {
        // Initialize visuals: requirements shown, fulfilled hidden
        if(requirementsImage != null)
        {
            requirementsImage.enabled = true;
            // If a slot restriction exists and has a sprite, use it.
            if(slotRestriction != null && slotRestriction.requirementSprite != null)
            {
                requirementsImage.sprite = slotRestriction.requirementSprite;
            }
        }
        if(fulfilledImage != null) fulfilledImage.enabled = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"OnDrop called on slot: {gameObject.name}");
        
        var draggedDice = eventData.pointerDrag;
        if (draggedDice != null && draggedDice.GetComponent<DraggableDice>() != null)
        {
            Debug.Log($"Dice {draggedDice.name} dropped on {gameObject.name}, DraggableDice will handle re-parenting.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Optional: Highlight the slot or provide feedback
        Debug.Log("Pointer entered the dice slot.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Optional: Remove highlight or feedback
        Debug.Log("Pointer exited the dice slot.");
    }

    public bool CanAcceptDice(Dice dice)
    {
        if (slotRestriction == null)
        {
            return true;
        }

        return slotRestriction.CheckDice(dice);
    }

    public bool AssignDice(GameObject diceObj)
    {
        // Check if requirements already fulfilled for this slot
        if (isRequirementFulfilled)
        {
            Debug.Log($"Slot {gameObject.name} requirements already fulfilled. No further assignments accepted.");
            return false;
        }

        DiceUI diceUI = diceObj.GetComponent<DiceUI>();
        if (diceUI == null)
        {
            Debug.LogWarning($"Cannot find DiceUI on {diceObj.name}. Cannot validate restrictions.");
            return false;
        }

        Dice diceData = diceUI.dataReference;
        if (diceData == null)
        {
            Debug.LogWarning($"Dice dataReference not found on {diceObj.name}, cannot validate restrictions.");
            return false;
        }

        if (!CanAcceptDice(diceData))
        {
            Debug.Log($"Dice {diceObj.name} does NOT meet the slot restrictions of {gameObject.name}.");
            return false;
        }

        currentDice = diceObj;
        currentDice.transform.SetParent(transform, false);

        var rectTransform = diceObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;
        
        // Mark the dice as assigned
        diceData.IsAssignedToSlot = true;

        Debug.Log($"Dice {diceObj.name} assigned to slot {gameObject.name}, new parent: {diceObj.transform.parent.name}");
        return true;
    }

    public void ClearSlot()
    {
        if (currentDice != null)
        {
            var diceUI = currentDice.GetComponent<DiceUI>();
            if(diceUI != null && diceUI.dataReference != null)
            {
                diceUI.dataReference.IsAssignedToSlot = false;
            }

            currentDice = null;
            Debug.Log($"Slot {gameObject.name} cleared.");
        }
    }

    public GameObject GetCurrentDice()
    {
        return currentDice;
    }
}
