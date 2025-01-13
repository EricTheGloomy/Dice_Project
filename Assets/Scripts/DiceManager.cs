using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour, IManager
{
    public GameObject dicePrefab;
    public GameObject diceUIContainerPrefab;
    public Canvas canvas;

    public DiceFaceSO[] diceFaces;
    public DiceColorSO[] diceColors;
    public StartingDiceSO startingDiceConfig;

    private Transform diceUIContainer;
    public List<Dice> dicePool;

    public void Initialize(GameController controller)
    {
        // Instantiate the UI container under the Canvas
        GameObject containerInstance = Instantiate(diceUIContainerPrefab, canvas.transform);
        diceUIContainer = containerInstance.transform;

        InitializeDicePool();
        Debug.Log("DiceManager initialized.");
    }

    public void InitializeDicePool()
    {
        dicePool = new List<Dice>();

        // Use StartingDiceSO to define dice pool
        foreach (var entry in startingDiceConfig.startingDice)
        {
            for (int i = 0; i < entry.count; i++)
            {
                var newDice = new Dice(entry.color, diceFaces);
                dicePool.Add(newDice);
                InstantiateDiceUI(newDice);
            }
        }

        Debug.Log($"Dice pool initialized with {dicePool.Count} dice.");
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
        dice.CurrentValue = Random.Range(1, diceFaces.Length + 1); // Roll between 1 and max faces
        UpdateDiceUI(dice);
        Debug.Log($"Dice rolled: {dice.LogicalColor.Name} -> {dice.CurrentValue}");
    }

    public void AddPip(Dice dice)
    {
        dice.CurrentValue = Mathf.Min(dice.CurrentValue + 1, diceFaces.Length); // Ensure it doesn’t exceed max faces
        UpdateDiceUI(dice);
        Debug.Log($"Added pip to dice: {dice.CurrentValue}");
    }

    public void SubtractPip(Dice dice)
    {
        dice.CurrentValue = Mathf.Max(dice.CurrentValue - 1, 1); // Ensure it doesn’t go below 1
        UpdateDiceUI(dice);
        Debug.Log($"Subtracted pip from dice: {dice.CurrentValue}");
    }

    public void InstantiateDiceUI(Dice dice)
    {
        GameObject diceUI = Instantiate(dicePrefab, diceUIContainer);

        // Set sprite or color based on DiceColorSO
        var image = diceUI.GetComponentInChildren<UnityEngine.UI.Image>();
        image.color = dice.LogicalColor.DisplayColor;

        dice.UIContainerObject = diceUI; // Store reference to update later
    }

    public void UpdateDiceUI(Dice dice)
    {
        if (dice.UIContainerObject != null)
        {
            var image = dice.UIContainerObject.GetComponentInChildren<UnityEngine.UI.Image>();
            image.sprite = dice.CurrentSprite;
        }
    }
}
