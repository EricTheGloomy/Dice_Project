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


        if (Input.GetKeyDown(KeyCode.L))
        {
            diceManager.ModifyPips(dice => true, 1); // Add 1 pip to all dice
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
           diceManager.ModifyPips(dice => true, -1); // Remove 1 pip from all dice 
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            var redColorSO = diceManager.GetColor(DiceColor.Red);
            diceManager.ModifyPips(dice => dice.LogicalColor == redColorSO, 1); // Add 1 pip to all red dice
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            var greenColorSO = diceManager.GetColor(DiceColor.Green);
            diceManager.ModifyPips(dice => dice.LogicalColor == greenColorSO, -1); // Remove 1 pip from all green dice
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            var randomDice = diceManager.dicePool[Random.Range(0, diceManager.dicePool.Count)];
            diceManager.ModifyPips(dice => dice == randomDice, 1); // Add 1 pip to a random dice
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            var randomDice = diceManager.dicePool[Random.Range(0, diceManager.dicePool.Count)];
            diceManager.ModifyPips(dice => dice == randomDice, -1); // Remove 1 pip from a random dice
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            var redColorSO = diceManager.GetColor(DiceColor.Red);
            var diceOfColor = diceManager.dicePool.FindAll(dice => dice.LogicalColor == redColorSO);
            if (diceOfColor.Count > 0)
            {
                var randomDice = diceOfColor[Random.Range(0, diceOfColor.Count)];
                diceManager.ModifyPips(dice => dice == randomDice, 1); // Add 1 pip to a random red dice
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            var greenColorSO = diceManager.GetColor(DiceColor.Green);
            var diceOfColor = diceManager.dicePool.FindAll(dice => dice.LogicalColor == greenColorSO);
            if (diceOfColor.Count > 0)
            {
                var randomDice = diceOfColor[Random.Range(0, diceOfColor.Count)];
                diceManager.ModifyPips(dice => dice == randomDice, -1); // Remove 1 pip from a random green dice
            }
        }


        if (Input.GetKeyDown(KeyCode.N))
        {
            diceManager.RollAllDice();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            EndTurn();
        }
    }

}
