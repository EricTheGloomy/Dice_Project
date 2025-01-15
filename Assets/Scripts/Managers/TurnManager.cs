using UnityEngine;

public class TurnManager : MonoBehaviour, IManager
{
    public ResourceManager resourceManager;
    public ResourceSO foodResource;
    public DiceManager diceManager;
    public DicePoolManager dicePoolManager;

    private int currentTurn;

    public void Initialize(GameController controller)
    {
        resourceManager = controller.resourceManager;
        diceManager = controller.diceManager;
        dicePoolManager = controller.dicePoolManager;
        Debug.Log("TurnManager initialized.");
    }

    public void StartTurn()
    {
        currentTurn++;
        Debug.Log($"Turn {currentTurn} started.");
    }

    public void EndTurn()
    {
        //TO DO - come back to this and refactor when locations are done and dice slots should be easier to find than by findobjectoftype
        foreach (var slot in FindObjectsOfType<DiceSlot>())
        {
            // Check if the slot has a dice that meets its restrictions
            bool requirementMet = false;
            if(slot.GetCurrentDice() != null)
            {
                var diceUI = slot.GetCurrentDice().GetComponent<DiceUI>();
                if(diceUI != null && diceUI.dataReference != null)
                {
                    // Use the slot's own restriction to check if the dice meets the requirement
                    requirementMet = slot.CanAcceptDice(diceUI.dataReference);
                }
            }

            if(requirementMet)
            {
                // Update visuals: hide requirements image, show fulfilled image
                if(slot.requirementsImage != null) slot.requirementsImage.enabled = false;
                if(slot.fulfilledImage != null) slot.fulfilledImage.enabled = true;
                // Lock this slot from further assignments for future turns
                slot.isRequirementFulfilled = true;
            }
            else
            {
                // Reset visuals in case the requirement isn't met at end of turn
                if(slot.requirementsImage != null) slot.requirementsImage.enabled = true;
                if(slot.fulfilledImage != null) slot.fulfilledImage.enabled = false;
                slot.isRequirementFulfilled = false;
            }
        }

        foreach (var dice in dicePoolManager.dicePool)
        {
            if(dice.UIContainerObject != null)
            {
                var draggable = dice.UIContainerObject.GetComponent<DraggableDice>();
                if (draggable != null)
                {
                    draggable.ResetToOriginalParent();
                }
                
                dice.IsAssignedToSlot = false;
                
                dice.UIContainerObject.SetActive(false);
            }
        }

        dicePoolManager.ClearTemporaryDice();
        Debug.Log($"Turn {currentTurn} ended.");
    }

}
