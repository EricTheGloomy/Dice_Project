using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour, IManager
{
    public DiceFaceSO[] diceFaces;
    public DiceColorSO[] diceColors;
    public StartingDiceSO startingDiceConfig;

    private UIManager uiManager;
    private Dictionary<DiceColor, DiceColorSO> colorLookup;

    public void Initialize(GameController controller)
    {
        uiManager = controller.uiManager;
        InitializeColorLookup();
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

    public Dice CreateDice(DiceColorSO color, bool isPermanent)
    {
        return new Dice(color, diceFaces, isPermanent);
    }

    public void RollDice(Dice dice)
    {
        // Skip dice that have been used this turn
        if (dice.IsUsedThisTurn)
        {
            Debug.Log($"Dice {dice.UIContainerObject.name} is used this turn; skipping roll.");
            return;
        }

        dice.CurrentValue = Random.Range(1, diceFaces.Length + 1);

        if(dice.UIContainerObject != null)
        {
            dice.UIContainerObject.SetActive(true);
        }

        uiManager.UpdateDiceUI(dice.UIContainerObject, dice.CurrentSprite);
    }

    public void RollAllDice(List<Dice> diceList)
    {
        foreach (var dice in diceList)
        {
            if (dice.IsAssignedToSlot)
            {
                continue;
            }

            RollDice(dice);
        }
    }
}
