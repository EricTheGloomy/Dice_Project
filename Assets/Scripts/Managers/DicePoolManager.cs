using System.Collections.Generic;
using UnityEngine;

public class DicePoolManager : MonoBehaviour, IManager
{
    private DiceManager diceManager;
    private UIManager uiManager;

    public List<Dice> dicePool = new List<Dice>();
    private List<Dice> tempDicePool = new List<Dice>();

    public void Initialize(GameController controller)
    {
        diceManager = controller.diceManager;
        uiManager = controller.uiManager;
        InitializeDicePool();
        Debug.Log("DicePoolManager initialized.");
    }

    private void InitializeDicePool()
    {
        dicePool.Clear();
        tempDicePool.Clear();

        foreach (var entry in diceManager.startingDiceConfig.startingDice)
        {
            var colorSO = diceManager.GetColor(entry.colorEnum);
            if (colorSO == null)
            {
                Debug.LogWarning($"No DiceColorSO found for {entry.colorEnum}");
                continue;
            }

            for (int i = 0; i < entry.count; i++)
            {
                AddDice(colorSO, entry.isPermanent);
            }
        }
    }

    public void AddDice(DiceColorSO color, bool isPermanent)
    {
        var newDice = diceManager.CreateDice(color, isPermanent);
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
        var newDice = diceManager.CreateDice(color, isPermanent);
        newDice.CurrentValue = Mathf.Clamp(startingFaceValue, 1, diceManager.diceFaces.Length);

        dicePool.Add(newDice);
        if (!isPermanent)
        {
            tempDicePool.Add(newDice);
        }

        var diceUI = uiManager.CreateDiceUI(newDice);

        newDice.UIContainerObject = diceUI;
        uiManager.UpdateDiceUI(diceUI, newDice.CurrentSprite);
        
        //TEST
        if (!isPermanent)
        {
            newDice.UIContainerObject.SetActive(true);
        }
        //TEST
    }

    public void RollAllDice()
    {
        diceManager.RollAllDice(dicePool);
    }

    public void ModifyPips(System.Func<Dice, bool> filter, int pipChange)
    {
        var targetDice = dicePool.FindAll(dice => filter(dice));

        foreach (var dice in targetDice)
        {
            dice.CurrentValue = Mathf.Clamp(dice.CurrentValue + pipChange, 1, diceManager.diceFaces.Length);
            uiManager.UpdateDiceUI(dice.UIContainerObject, dice.CurrentSprite);
        }

        Debug.Log($"Modified pips by {pipChange} for {targetDice.Count} dice.");
    }

    public void AddPip(Dice dice)
    {
        dice.CurrentValue = Mathf.Min(dice.CurrentValue + 1, diceManager.diceFaces.Length);
        uiManager.UpdateDiceUI(dice.UIContainerObject, dice.CurrentSprite);
        Debug.Log($"Added pip to dice: {dice.CurrentValue}");
    }

    public void SubtractPip(Dice dice)
    {
        dice.CurrentValue = Mathf.Max(dice.CurrentValue - 1, 1);
        uiManager.UpdateDiceUI(dice.UIContainerObject, dice.CurrentSprite);
        Debug.Log($"Subtracted pip from dice: {dice.CurrentValue}");
    }

    public void RemoveDice(Dice dice)
    {
        if (dicePool.Contains(dice))
        {
            dicePool.Remove(dice);
            if (tempDicePool.Contains(dice))
            {
                tempDicePool.Remove(dice);
            }

            if (dice.UIContainerObject != null)
            {
                Destroy(dice.UIContainerObject);
            }
        }
    }

    public void ClearTemporaryDice()
    {
        foreach (var dice in tempDicePool)
        {
            if (dicePool.Contains(dice))
            {
                dicePool.Remove(dice);
                if (dice.UIContainerObject != null)
                {
                    Destroy(dice.UIContainerObject);
                }
            }
        }
        tempDicePool.Clear();
        Debug.Log("Temporary dice cleared.");
    }

    public void ResetPool()
    {
        foreach (var dice in dicePool)
        {
            if (dice.UIContainerObject != null)
            {
                Destroy(dice.UIContainerObject);
            }
        }

        dicePool.Clear();
        tempDicePool.Clear();
        Debug.Log("Dice pool reset.");
    }
}
