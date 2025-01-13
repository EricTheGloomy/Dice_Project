using UnityEngine;

public class TurnManager : MonoBehaviour, IManager
{
    public ResourceManager resourceManager;
    public ResourceSO foodResource;
    public DiceManager diceManager;

    private int currentTurn;

    public void Initialize(GameController controller)
    {
        resourceManager = controller.resourceManager;
        diceManager = controller.diceManager;
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddDice(DiceColor.Red);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddDice(DiceColor.White);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddDice(DiceColor.Green);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            diceManager.RollAllDice();
        }
    }

    private void AddDice(DiceColor color)
    {
        var colorSO = diceManager.GetColor(color);
        if (colorSO != null)
        {
            var newDice = new Dice(colorSO, diceManager.diceFaces);
            diceManager.dicePool.Add(newDice);
            diceManager.InstantiateDiceUI(newDice);
            diceManager.UpdateDiceUI(newDice);
        }
        else
        {
            Debug.LogWarning($"Color {color} not found in DiceManager.");
        }
    }
}
