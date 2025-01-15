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

        dicePoolManager.RollAllDice();
    }

    public void EndTurn()
    {
        dicePoolManager.ClearTemporaryDice();
        Debug.Log($"Turn {currentTurn} ended.");
    }

}
