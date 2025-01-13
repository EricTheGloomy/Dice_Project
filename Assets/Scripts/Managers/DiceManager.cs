using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private List<Dice> tempDicePool = new List<Dice>();


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
                var newDice = new Dice(colorSO, diceFaces, entry.isPermanent); // Pass isPermanent flag
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

        // Use the helper script to access the Image
        var diceUIComponent = diceUI.GetComponent<DiceUI>();

        if (diceUIComponent != null && diceUIComponent.faceImage != null)
        {
            diceUIComponent.faceImage.color = dice.LogicalColor.DisplayColor;
        }
        else
        {
            Debug.LogWarning("DiceUI component or faceImage not set up correctly in prefab.");
        }

        dice.UIContainerObject = diceUI; // Store reference to update later
    }

    public void UpdateDiceUI(Dice dice)
    {
        if (dice.UIContainerObject != null)
        {
            // Use the DiceUI helper script to access the Image component
            var diceUIComponent = dice.UIContainerObject.GetComponent<DiceUI>();

            if (diceUIComponent != null && diceUIComponent.faceImage != null)
            {
                diceUIComponent.faceImage.sprite = dice.CurrentSprite; // Update the sprite
            }
            else
            {
                Debug.LogWarning("DiceUI component or faceImage not set up correctly on this dice.");
            }
        }
        else
        {
            Debug.LogWarning("UIContainerObject is null for this dice.");
        }
    }

    public void AddDice(DiceColorSO color, bool isPermanent)
    {
        var newDice = new Dice(color, diceFaces, isPermanent);
        dicePool.Add(newDice);

        if (!isPermanent)
        {
            tempDicePool.Add(newDice);
        }

        InstantiateDiceUI(newDice);
        UpdateDiceUI(newDice);
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
        InstantiateDiceUI(newDice);
        UpdateDiceUI(newDice);
    }

    public void ModifyPips(System.Func<Dice, bool> filter, int pipChange)
    {
        // Filter dice that match the criteria and can be safely modified
        var targetDice = dicePool.FindAll(new System.Predicate<Dice>(dice =>
        {
            // Apply the filter provided
            if (!filter(dice))
            {
                return false; // Skip dice that don't match the filter
            }

            // Check if the dice can handle the pip change
            if (pipChange < 0) // Negative change
            {
                return dice.CurrentValue > 1; // Exclude dice already at 1
            }
            else if (pipChange > 0) // Positive change
            {
                return dice.CurrentValue < diceFaces.Length; // Exclude dice already at max
            }

            return false; // If pipChange is 0, do nothing
        }));

        // Modify each valid dice
        foreach (var dice in targetDice)
        {
            dice.CurrentValue = Mathf.Clamp(dice.CurrentValue + pipChange, 1, diceFaces.Length);
            UpdateDiceUI(dice);
        }

        Debug.Log($"Modified pips by {pipChange} for {targetDice.Count} dice.");
    }

    public void ClearTemporaryDice()
    {
        foreach (var dice in tempDicePool)
        {
            Destroy(dice.UIContainerObject); // Remove UI
            dicePool.Remove(dice);           // Remove from main pool
        }

        tempDicePool.Clear();
        Debug.Log("Temporary dice cleared.");
    }

}
