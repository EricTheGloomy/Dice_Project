using UnityEngine;

public class TurnManager : MonoBehaviour, IManager
{
    public ResourceManager resourceManager;
    public ResourceSO foodResource;
    public DiceManager diceManager;
    public DicePoolManager dicePoolManager;
    public LocationDeckManager locationDeckManager;
    public SkillManager skillManager;

    private int currentTurn;

    public void Initialize(GameController controller)
    {
        resourceManager = controller.resourceManager;
        diceManager = controller.diceManager;
        dicePoolManager = controller.dicePoolManager;
        locationDeckManager = controller.locationDeckManager;
        skillManager = controller.skillManager;
        Debug.Log("TurnManager initialized.");
    }

    public void StartTurn()
    {
        currentTurn++;
        Debug.Log($"Turn {currentTurn} started.");

        // Reset any dice used last turn so they can be used again
        foreach (var dice in dicePoolManager.dicePool)
        {
            dice.IsUsedThisTurn = false;
            dice.IsAssignedToSlot = false;

            // Optionally hide them until the player chooses to roll
            if (dice.UIContainerObject != null)
            {
                dice.UIContainerObject.SetActive(false);
            }
        }

        // (Previously, ApplyOngoingEffects was here. Move it to EndTurn if you want end-of-turn effect)
    }
    
    public void EndTurn()
    {
        // Let the deck manager check all slots (and possibly finalize location completions)
        locationDeckManager.CheckAllSlotsOnActiveCards();

        // Return dice to their original parent and hide them (only if you want to "clean up")
        foreach (var dice in dicePoolManager.dicePool)
        {
            if(dice.UIContainerObject != null)
            {
                var draggable = dice.UIContainerObject.GetComponent<DraggableDice>();
                if (draggable != null)
                {
                    draggable.ResetToOriginalParent();
                }
                dice.UIContainerObject.SetActive(false);
                dice.IsAssignedToSlot = false;
            }
        }

        // Clear any temporary dice
        dicePoolManager.ClearTemporaryDice();

        // Ongoing effects for each incomplete location happen now (end of turn):
        if (locationDeckManager != null)
        {
            locationDeckManager.ApplyOngoingEffects(resourceManager, /* e.g. populationResource */ null);
            locationDeckManager.CheckCardResolutions(resourceManager, /* goldResource */ null);
        }

        Debug.Log($"Turn {currentTurn} ended.");
    }
}
