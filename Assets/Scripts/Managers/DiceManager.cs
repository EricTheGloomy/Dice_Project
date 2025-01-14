using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour, IManager
{
    public DiceFaceSO[] diceFaces;
    public DiceColorSO[] diceColors;
    public StartingDiceSO startingDiceConfig;

    private UIManager uiManager;
    private Dictionary<DiceColor, DiceColorSO> colorLookup;
    public List<Dice> dicePool;
    private List<Dice> tempDicePool = new List<Dice>();

    public void Initialize(GameController controller)
    {
        uiManager = controller.uiManager;

        InitializeColorLookup();
        InitializeDicePool();
        Debug.Log("DiceManager initialized.");
    }

    private void InitializeColorLookup()
    {
        colorLookup = new Dictionary<DiceColor, DiceColorSO>();
        foreach (var colorSO in diceColors)
        {
            colorLookup[colorSO.ColorEnum] = colorSO;
        }
        Debug.Log("Color lookup dictionary initialized.");
    }

    public DiceColorSO GetColor(DiceColor color)
    {
        if (colorLookup.TryGetValue(color, out var colorSO))
        {
            return colorSO;
        }
        Debug.LogWarning($"Color {color} not found in colorLookup dictionary.");
        return null;
    }

    private void InitializeDicePool()
    {
        dicePool = new List<Dice>();

        foreach (var entry in startingDiceConfig.startingDice)
        {
            if (!colorLookup.ContainsKey(entry.colorEnum))
            {
                Debug.LogWarning($"No DiceColorSO found for {entry.colorEnum}");
                continue;
            }

            var colorSO = colorLookup[entry.colorEnum];
            for (int i = 0; i < entry.count; i++)
            {
                var newDice = new Dice(colorSO, diceFaces, entry.isPermanent);
                dicePool.Add(newDice);

                // Delegate UI creation to UIManager
                var diceUI = uiManager.CreateDiceUI(newDice);
                newDice.UIContainerObject = diceUI;
            }
        }
    }

    public void RollAllDice()
    {
        foreach (var dice in dicePool)
        {
            RollDice(dice);
        }
    }

    public void RollDice(Dice dice)
    {
        dice.CurrentValue = Random.Range(1, diceFaces.Length + 1);
        uiManager.UpdateDiceUI(dice.UIContainerObject, dice.CurrentSprite);
    }

    public void AddPip(Dice dice)
    {
        dice.CurrentValue = Mathf.Min(dice.CurrentValue + 1, diceFaces.Length);
        uiManager.UpdateDiceUI(dice.UIContainerObject, dice.CurrentSprite);
        Debug.Log($"Added pip to dice: {dice.CurrentValue}");
    }

    public void SubtractPip(Dice dice)
    {
        dice.CurrentValue = Mathf.Max(dice.CurrentValue - 1, 1);
        uiManager.UpdateDiceUI(dice.UIContainerObject, dice.CurrentSprite);
        Debug.Log($"Subtracted pip from dice: {dice.CurrentValue}");
    }

    public void AddDice(DiceColorSO color, bool isPermanent)
    {
        var newDice = new Dice(color, diceFaces, isPermanent);
        dicePool.Add(newDice);
        if (!isPermanent)
        {
            tempDicePool.Add(newDice);
        }

        var diceUI = uiManager.CreateDiceUI(newDice);
        newDice.UIContainerObject = diceUI;
        uiManager.UpdateDiceUI(diceUI, newDice.CurrentSprite);
    }

    public void AddDiceWithFaceValue(DiceColorSO color, bool isPermanent, int startingFaceValue)
    {
        var newDice = new Dice(color, diceFaces, isPermanent);
        newDice.CurrentValue = Mathf.Clamp(startingFaceValue, 1, diceFaces.Length);
        dicePool.Add(newDice);
        if (!isPermanent)
        {
            tempDicePool.Add(newDice);
        }

        var diceUI = uiManager.CreateDiceUI(newDice);
        newDice.UIContainerObject = diceUI;
        uiManager.UpdateDiceUI(diceUI, newDice.CurrentSprite);
    }

    public void ModifyPips(System.Func<Dice, bool> filter, int pipChange)
    {
        var targetDice = dicePool.FindAll(dice => filter(dice));

        foreach (var dice in targetDice)
        {
            dice.CurrentValue = Mathf.Clamp(dice.CurrentValue + pipChange, 1, diceFaces.Length);
            uiManager.UpdateDiceUI(dice.UIContainerObject, dice.CurrentSprite);
        }

        Debug.Log($"Modified pips by {pipChange} for {targetDice.Count} dice.");
    }

    public void ClearTemporaryDice()
    {
        uiManager.ClearDiceUI(tempDicePool);
        foreach (var dice in tempDicePool)
        {
            dicePool.Remove(dice);
        }
        tempDicePool.Clear();
        Debug.Log("Temporary dice cleared.");
    }
}
