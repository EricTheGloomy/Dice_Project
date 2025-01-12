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

//TESTING PURPOSES ONLY
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Dice newDice = new Dice(diceManager.diceColors[0], diceManager.diceFaces);
            diceManager.dicePool.Add(newDice); // Add the new dice to the pool
            diceManager.InstantiateDiceUI(newDice); // Instantiate the UI for the new dice
            diceManager.UpdateDiceUI(newDice);
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            Dice newDice = new Dice(diceManager.diceColors[1], diceManager.diceFaces);
            diceManager.dicePool.Add(newDice); // Add the new dice to the pool
            diceManager.InstantiateDiceUI(newDice); // Instantiate the UI for the new dice
            diceManager.UpdateDiceUI(newDice);
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            Dice newDice = new Dice(diceManager.diceColors[2], diceManager.diceFaces);
            diceManager.dicePool.Add(newDice); // Add the new dice to the pool
            diceManager.InstantiateDiceUI(newDice); // Instantiate the UI for the new dice
            diceManager.UpdateDiceUI(newDice);
        }
        if(Input.GetKeyDown(KeyCode.U))
        {
            diceManager.RollAllDice();
        }
    }
}
