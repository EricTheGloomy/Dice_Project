using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Slot Restriction")]
    [Tooltip("Assign a DiceSlotRestrictionSO here if this slot is restricted.")]
    public DiceSlotRestrictionSO slotRestriction;
    
    [Header("Requirement Description")]
    [Tooltip("TextMesh Pro component to display requirement description.")]
    public TextMeshProUGUI requirementDescriptionText;

    [Header("Requirement Visuals")]
    public Image requirementsImage;   // Image showing requirements
    public Image fulfilledImage;      // Image to show when fulfilled

    private GameObject currentDice;
    public bool isRequirementFulfilled = false;

    public bool IsEmpty => currentDice == null;

    private void Awake()
    {
        // Initialize visuals: requirements shown, fulfilled hidden
        if(requirementsImage != null)
        {
            requirementsImage.enabled = true;
            if(slotRestriction != null && slotRestriction.requirementSprite != null)
            {
                requirementsImage.sprite = slotRestriction.requirementSprite;
            }
        }
        if(fulfilledImage != null) fulfilledImage.enabled = false;

        // Set requirement description text if available
        if(requirementDescriptionText != null && slotRestriction != null)
        {
            requirementDescriptionText.text = slotRestriction.requirementDescription;
        }
    }

    public void SetRestriction(DiceSlotRestrictionSO newRestriction)
    {
        slotRestriction = newRestriction;

        // Update requirement image
        if (requirementsImage != null && newRestriction != null && newRestriction.requirementSprite != null)
        {
            requirementsImage.sprite = newRestriction.requirementSprite;
        }

        // Update the requirement description text
        if (requirementDescriptionText != null)
        {
            if (newRestriction != null && !string.IsNullOrEmpty(newRestriction.requirementDescription))
            {
                requirementDescriptionText.text = newRestriction.requirementDescription;
            }
            else
            {
                requirementDescriptionText.text = ""; 
            }
        }
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
        Debug.Log("Pointer entered the dice slot.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
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

        // Mark this slot as fulfilled 
        isRequirementFulfilled = true;
        FulfillSlotVisuals();
        diceObj.SetActive(false);

        Debug.Log($"Dice {diceObj.name} assigned to slot {gameObject.name}, new parent: {diceObj.transform.parent.name}");

        // Once the final slot is fulfilled, check if the entire location is done
        var locationCardUI = GetComponentInParent<LocationCardUI>();
        if (locationCardUI != null && locationCardUI.IsCardFulfilled())
        {
            // Find the TurnManager (or ResourceManager) in the scene
            TurnManager turnManager = FindObjectOfType<TurnManager>();
            ResourceManager rm = FindObjectOfType<ResourceManager>();

            // If found, award gold
            if (turnManager != null && rm != null && turnManager.goldResource != null)
            {
                rm.AddResource(turnManager.goldResource, locationCardUI.cardData.goldReward);
                Debug.Log($"Immediate awarding {locationCardUI.cardData.goldReward} gold for completing {locationCardUI.cardData.locationName}!");
            }

            // Optionally hide or flip the location
            locationCardUI.ShowFront(false);
        }

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

            // Mark it as unfulfilled again
            isRequirementFulfilled = false;
            Debug.Log($"Slot {gameObject.name} cleared.");
        }
    }

    public GameObject GetCurrentDice()
    {
        return currentDice;
    }

    public void FulfillSlotVisuals()
    {
        isRequirementFulfilled = true;
        
        // Hide the requirement text
        if (requirementDescriptionText != null)
        {
            requirementDescriptionText.gameObject.SetActive(false);
        }

        // Hide the requirement sprite
        if (requirementsImage != null)
        {
            requirementsImage.enabled = false;
        }

        // Show the fulfilled sprite
        if (fulfilledImage != null)
        {
            fulfilledImage.enabled = true;
        }
    }

    public void UnfulfillSlotVisuals()
    {
        isRequirementFulfilled = false;
        
        // Show requirement text
        if (requirementDescriptionText != null)
        {
            requirementDescriptionText.gameObject.SetActive(true);
        }

        // Show requirement sprite
        if (requirementsImage != null)
        {
            requirementsImage.enabled = true;
        }

        // Hide the fulfilled sprite
        if (fulfilledImage != null)
        {
            fulfilledImage.enabled = false;
        }
    }
}
