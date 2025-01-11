using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    private List<Dice> dicePool;

    public void InitializeDicePool()
    {
        dicePool = new List<Dice>
        {
            new Dice("Red"),
            new Dice("Blue"),
            new Dice("Green")
        };
        Debug.Log("Dice pool initialized.");
    }

    public void RollAllDice()
    {
        foreach (var dice in dicePool)
        {
            dice.Roll();
            Debug.Log($"Dice rolled: {dice.LogicalColor} -> {dice.CurrentValue}");
        }
    }
}