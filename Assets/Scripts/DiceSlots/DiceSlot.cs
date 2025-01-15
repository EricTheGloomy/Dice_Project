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
        // We could add any initialization here
    }

    public void OnDrop(PointerEventData eventData)
    {
        // We do not do the re-parenting here, because DraggableDice handles that.
        Debug.Log($"OnDrop called on slot: {gameObject.name}");
        
        var draggedDice = eventData.pointerDrag;
        if (draggedDice != null && draggedDice.GetComponent<DraggableDice>() != null)
        {
            Debug.Log($"Dice {draggedDice.name} dropped on {gameObject.name}, DraggableDice will handle re-parenting.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Optional: highlight the slot or do some feedback
        Debug.Log("Pointer entered the dice slot.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Optional: remove highlight
        Debug.Log("Pointer exited the dice slot.");
    }

    /// <summary>
    /// Checks if a given dice (data) passes the assigned restriction (if any).
    /// Returns true if either no restriction is assigned or all checks pass.
    /// </summary>
    public bool CanAcceptDice(Dice dice)
    {
        // If no restriction is assigned, default to true
        if (slotRestriction == null)
        {
            return true;
        }

        // Otherwise, check our single restriction object
        return slotRestriction.CheckDice(dice);
    }

    /// <summary>
    /// AssignDice is called by DraggableDice once it has decided to place 
    /// the dice in this slot (assuming the restrictions pass).
    /// </summary>
    public void AssignDice(GameObject diceObj)
    {
        // 1) Find the Dice data from the UI
        DiceUI diceUI = diceObj.GetComponent<DiceUI>();
        if (diceUI == null)
        {
            Debug.LogWarning($"Cannot find DiceUI on {diceObj.name}. Cannot validate restrictions.");
            return;
        }

        Dice diceData = diceUI.dataReference; // The actual data object
        if (diceData == null)
        {
            Debug.LogWarning($"Dice dataReference not found on {diceObj.name}, cannot validate restrictions.");
            return;
        }

        // 2) Check if we can accept it
        if (!CanAcceptDice(diceData))
        {
            Debug.Log($"Dice {diceObj.name} does NOT meet the slot restrictions of {gameObject.name}.");
            // We do NOT assign the dice if it fails
            return;
        }

        // 3) If restrictions pass, assign the dice
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
