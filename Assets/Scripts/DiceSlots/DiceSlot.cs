using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// DiceSlot can have a single DiceSlotRestrictionSO to define which dice are allowed.
/// </summary>
public class DiceSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Slot Restriction")]
    [Tooltip("Assign a DiceSlotRestrictionSO here if this slot is restricted.")]
    public DiceSlotRestrictionSO slotRestriction;

    private GameObject currentDice;

    public bool IsEmpty => currentDice == null;

    private void Awake()
    {
        // Initialization if needed
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

    /// <summary>
    /// Checks if a given dice (data) passes the assigned restriction (if any).
    /// Returns true if either no restriction is assigned or all checks pass.
    /// </summary>
    public bool CanAcceptDice(Dice dice)
    {
        if (slotRestriction == null)
        {
            return true;
        }

        return slotRestriction.CheckDice(dice);
    }

    /// <summary>
    /// Attempts to assign the dice to this slot.
    /// Returns true if assignment is successful, false otherwise.
    /// </summary>
    public bool AssignDice(GameObject diceObj)
    {
        // 1) Find the Dice data from the UI
        DiceUI diceUI = diceObj.GetComponent<DiceUI>();
        if (diceUI == null)
        {
            Debug.LogWarning($"Cannot find DiceUI on {diceObj.name}. Cannot validate restrictions.");
            return false;
        }

        Dice diceData = diceUI.dataReference; // The actual data object
        if (diceData == null)
        {
            Debug.LogWarning($"Dice dataReference not found on {diceObj.name}, cannot validate restrictions.");
            return false;
        }

        // 2) Check if we can accept it
        if (!CanAcceptDice(diceData))
        {
            Debug.Log($"Dice {diceObj.name} does NOT meet the slot restrictions of {gameObject.name}.");
            // Do not assign the dice if it fails
            return false;
        }

        // 3) Assign the dice
        currentDice = diceObj;
        currentDice.transform.SetParent(transform, false);

        // Reset the RectTransform to center
        var rectTransform = diceObj.GetComponent<RectTransform>();
        rectTransform.anchorMin         = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax         = new Vector2(0.5f, 0.5f);
        rectTransform.pivot             = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition  = Vector2.zero;
        rectTransform.localRotation     = Quaternion.identity;
        rectTransform.localScale        = Vector3.one;
        
        Debug.Log($"Dice {diceObj.name} assigned to slot {gameObject.name}, new parent: {diceObj.transform.parent.name}");
        return true;
    }

    public void ClearSlot()
    {
        if (currentDice != null)
        {
            currentDice = null;
            Debug.Log($"Slot {gameObject.name} cleared.");
        }
    }

    public GameObject GetCurrentDice()
    {
        return currentDice;
    }
}
