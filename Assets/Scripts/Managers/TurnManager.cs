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

//TESTING PURPOSES ONLY
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            var colorSO = diceManager.GetColor(DiceColor.Red);
            if (colorSO != null)
            {
                //diceManager.AddDice(colorSO, false);
                dicePoolManager.AddDiceWithFaceValue(colorSO, false, 6);
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            var colorSO = diceManager.GetColor(DiceColor.White);
            if (colorSO != null)
            {
                //diceManager.AddDice(colorSO, false);
                dicePoolManager.AddDiceWithFaceValue(colorSO, false, 2);
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            var colorSO = diceManager.GetColor(DiceColor.Green);
            if (colorSO != null)
            {
                //diceManager.AddDice(colorSO, false);
                dicePoolManager.AddDiceWithFaceValue(colorSO, false, 4);
            }
        }


        if (Input.GetKeyDown(KeyCode.L))
        {
            dicePoolManager.ModifyPips(dice => true, 1); // Add 1 pip to all dice
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
           dicePoolManager.ModifyPips(dice => true, -1); // Remove 1 pip from all dice 
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            var redColorSO = diceManager.GetColor(DiceColor.Red);
            dicePoolManager.ModifyPips(dice => dice.LogicalColor == redColorSO, 1); // Add 1 pip to all red dice
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            var greenColorSO = diceManager.GetColor(DiceColor.Green);
            dicePoolManager.ModifyPips(dice => dice.LogicalColor == greenColorSO, -1); // Remove 1 pip from all green dice
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            var randomDice = dicePoolManager.dicePool[Random.Range(0, dicePoolManager.dicePool.Count)];
            dicePoolManager.ModifyPips(dice => dice == randomDice, 1); // Add 1 pip to a random dice
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            var randomDice = dicePoolManager.dicePool[Random.Range(0, dicePoolManager.dicePool.Count)];
            dicePoolManager.ModifyPips(dice => dice == randomDice, -1); // Remove 1 pip from a random dice
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            var redColorSO = diceManager.GetColor(DiceColor.Red);
            var diceOfColor = dicePoolManager.dicePool.FindAll(dice => dice.LogicalColor == redColorSO);
            if (diceOfColor.Count > 0)
            {
                var randomDice = diceOfColor[Random.Range(0, diceOfColor.Count)];
                dicePoolManager.ModifyPips(dice => dice == randomDice, 3); // Add 1 pip to a random red dice
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            var greenColorSO = diceManager.GetColor(DiceColor.Green);
            var diceOfColor = dicePoolManager.dicePool.FindAll(dice => dice.LogicalColor == greenColorSO);
            if (diceOfColor.Count > 0)
            {
                var randomDice = diceOfColor[Random.Range(0, diceOfColor.Count)];
                dicePoolManager.ModifyPips(dice => dice == randomDice, -3); // Remove 1 pip from a random green dice
            }
        }
        if (Input.GetKeyDown(KeyCode.Z)) // Test +3
        {
            int pipChange = 3; // Change this to -3 for testing negatives
            var redColorSO = diceManager.GetColor(DiceColor.Red);
            var eligibleDice = dicePoolManager.dicePool.FindAll(dice =>
                dice.LogicalColor == redColorSO && // Match color
                !(dice.CurrentValue == diceManager.diceFaces.Length && pipChange > 0) && // Exclude 6 for +
                !(dice.CurrentValue == 1 && pipChange < 0) // Exclude 1 for -
            );

            if (eligibleDice.Count > 0)
            {
                var randomDice = eligibleDice[Random.Range(0, eligibleDice.Count)];
                dicePoolManager.ModifyPips(dice => dice == randomDice, pipChange);
            }
            else
            {
                Debug.LogWarning($"No eligible red dice for {pipChange:+#;-#;0}.");
            }
        }


        if (Input.GetKeyDown(KeyCode.N))
        {
            dicePoolManager.RollAllDice();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            EndTurn();
        }
    }

}
