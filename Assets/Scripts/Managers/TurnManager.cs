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

        // Apply ongoing effects from location cards here if you prefer:
        if (locationDeckManager != null)
        {
            locationDeckManager.ApplyOngoingEffects(resourceManager, /* populationResource */ null);
        }
    }

//TO DO - come back to this and refactor when locations are done and dice slots should be easier to find than by findobjectoftype
    public void EndTurn()
    {
        // Let the deck manager check all slots
        locationDeckManager.CheckAllSlotsOnActiveCards();

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

        // Check if any location cards were fulfilled this turn
        // awarding gold or spawning new cards
        if (locationDeckManager != null)
        {
            // Let's assume 'foodResource' is not relevant, so let's also define a 'goldResource' for simplicity
            // For demonstration, let's pass in resourceManager.GetResourceSOByName("Gold") or something similar
            // Or just create a public field in TurnManager referencing a gold ResourceSO
            locationDeckManager.CheckCardResolutions(resourceManager, /* goldResource */ null);
        }

        dicePoolManager.ClearTemporaryDice();
        Debug.Log($"Turn {currentTurn} ended.");

    }

}
