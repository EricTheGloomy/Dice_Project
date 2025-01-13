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
        diceManager.ClearTemporaryDice();
        Debug.Log($"Turn {currentTurn} ended.");
    }

//TESTING PURPOSES ONLY
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            var colorSO = diceManager.GetColor(DiceColor.Red);
            if (colorSO != null)
            {
                //diceManager.AddDice(colorSO, false);
                diceManager.AddDiceWithFaceValue(colorSO, false, 6);
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            var colorSO = diceManager.GetColor(DiceColor.White);
            if (colorSO != null)
            {
                //diceManager.AddDice(colorSO, false);
                diceManager.AddDiceWithFaceValue(colorSO, false, 2);
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            var colorSO = diceManager.GetColor(DiceColor.Green);
            if (colorSO != null)
            {
                //diceManager.AddDice(colorSO, false);
                diceManager.AddDiceWithFaceValue(colorSO, false, 4);
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            diceManager.RollAllDice();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            EndTurn();
        }
    }

}
