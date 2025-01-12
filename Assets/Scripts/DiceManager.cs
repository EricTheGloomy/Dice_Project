using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour, IManager
{
    public GameObject dicePrefab;          // Prefab with an Image component
    public Transform diceUIContainer;     // UI container for dice
    public DiceFaceSO[] diceFaces;        // Assign in Inspector (set 6 faces)
    public DiceColorSO[] diceColors;      // Assign in Inspector (e.g., Red, Blue, Green)

    private List<Dice> dicePool;

    public void Initialize(GameController controller)
    {
        InitializeDicePool();
        Debug.Log("DiceManager initialized.");
    }

    public void InitializeDicePool()
    {
        dicePool = new List<Dice>();

        foreach (var color in diceColors)
        {
            dicePool.Add(new Dice(color, diceFaces));
        }

        foreach (var dice in dicePool)
        {
            InstantiateDiceUI(dice);
        }

        Debug.Log("Dice pool initialized.");
    }

    public void RollAllDice()
    {
        foreach (var dice in dicePool)
        {
            dice.Roll();
            UpdateDiceUI(dice);
            Debug.Log($"Dice rolled: {dice.LogicalColor.Name} -> {dice.CurrentValue}");
        }
    }

    private void InstantiateDiceUI(Dice dice)
    {
        GameObject diceUI = Instantiate(dicePrefab, diceUIContainer);

        // Set sprite or color based on DiceColorSO
        var image = diceUI.GetComponentInChildren<UnityEngine.UI.Image>();
        image.color = dice.LogicalColor.DisplayColor;

        dice.UIContainerObject = diceUI; // Store reference to update later
    }

    private void UpdateDiceUI(Dice dice)
    {
        if (dice.UIContainerObject != null)
        {
            var image = dice.UIContainerObject.GetComponentInChildren<UnityEngine.UI.Image>();
            image.sprite = dice.CurrentSprite;
        }
    }
}
