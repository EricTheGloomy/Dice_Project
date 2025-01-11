using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    public ResourceSO foodResource;
    public DiceManager diceManager;

    private int currentTurn;

    public void StartTurn()
    {
        currentTurn++;
        Debug.Log($"Turn {currentTurn} started.");

        //for testing purposes only - resources will not decrease on turn start in the game
        resourceManager.DeductResource(foodResource, 1);
        int foodValue = resourceManager.GetResourceValue(foodResource);
        Debug.Log($"Food remaining: {foodValue}");

        diceManager.RollAllDice();
    }

    public void EndTurn()
    {
        Debug.Log($"Turn {currentTurn} ended.");
    }
}
