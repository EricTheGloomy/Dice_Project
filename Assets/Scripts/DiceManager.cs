using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour, IManager
{
    public GameObject dicePrefab;             // Prefab for individual dice
    public GameObject diceUIContainerPrefab;  // Prefab for the dice UI container
    public Canvas canvas;                     // Reference to the Canvas in the scene
    public DiceFaceSO[] diceFaces;            // Assign in Inspector (set 6 faces)
    public DiceColorSO[] diceColors;          // Assign in Inspector (e.g., Red, Blue, Green)

    private Transform diceUIContainer;        // Dynamically instantiated container
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
