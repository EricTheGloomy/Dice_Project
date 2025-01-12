using UnityEngine;

public class TurnManager : MonoBehaviour, IManager
{
    public ResourceManager resourceManager;
    public ResourceSO foodResource;
    public DiceManager diceManager;

    private int currentTurn;

    public void Initialize(GameController controller)
    {
        resourceManager = controller.resourceManager; // Retrieve dependency
        diceManager = controller.diceManager;         // Retrieve dependency
        Debug.Log("TurnManager initialized.");
    }

    public void StartTurn()
    {
        currentTurn++;
        Debug.Log($"Turn {currentTurn} started.");

        diceManager.RollAllDice();
    }

    public void EndTurn()
    {
        Debug.Log($"Turn {currentTurn} ended.");
    }
}
