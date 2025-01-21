using UnityEngine;

public class TurnManager : MonoBehaviour, IManager
{
    public ResourceManager resourceManager;
    public DiceManager diceManager;
    public DicePoolManager dicePoolManager;
    public LocationDeckManager locationDeckManager;
    public SkillManager skillManager;

    [Header("Resources Used for End Turn Effects")]
    public ResourceSO goldResource;       // <-- Assign your GoldResourceSO in Inspector
    public ResourceSO populationResource; // <-- Assign your PopulationResourceSO in Inspector

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

            if (dice.UIContainerObject != null)
            {
                dice.UIContainerObject.SetActive(false);
            }
        }

        // We do NOT apply location ongoing effects here, we do it at EndTurn
    }
    
    public void EndTurn()
    {
        Debug.Log($"Ending turn {currentTurn}...");

        // 1) Check all location dice slots and finalize them
        locationDeckManager.CheckAllSlotsOnActiveCards();

        // 2) Optionally reset dice UI positions
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
                dice.IsUsedThisTurn = false;
            }
        }

        // 3) Clear temporary dice
        dicePoolManager.ClearTemporaryDice();

        // 4) Apply ongoing effects to incomplete locations
        //    e.g. reduce population
        if (locationDeckManager != null)
        {
            locationDeckManager.ApplyOngoingEffects(resourceManager, populationResource);

            // 5) Now check if any card is fully completed and give gold
            //    This method is expecting to get goldResource as well
            //NOTE cards should rather reward gold immediately not here
            //locationDeckManager.CheckCardResolutions(resourceManager, goldResource);
        }

        // Step X: For each skill in your skillManager, clear its slots
        if (skillManager != null)
        {
            skillManager.ClearAllSkillSlots();
        }

        Debug.Log($"Turn {currentTurn} ended.");
    }
}
