using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    public DiceManager diceManager;

    private int currentTurn;

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