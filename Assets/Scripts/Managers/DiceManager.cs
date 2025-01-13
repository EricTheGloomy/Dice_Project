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
    private Dictionary<DiceColor, DiceColorSO> colorLookup; // Mapping enum to SO
    public List<Dice> dicePool;

    public void Initialize(GameController controller)
    {
        // Instantiate the UI container under the Canvas
        GameObject containerInstance = Instantiate(diceUIContainerPrefab, canvas.transform);
        diceUIContainer = containerInstance.transform;

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
    
    public void InitializeDicePool()
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
                var newDice = new Dice(colorSO, diceFaces);
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
