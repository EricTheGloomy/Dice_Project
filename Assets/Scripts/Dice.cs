using UnityEngine;

public class Dice
{
    public string LogicalColor { get; private set; }
    public int CurrentValue { get; private set; }

    public Dice(string color)
    {
        LogicalColor = color;
        CurrentValue = 1; // Default to 1
    }

    public void Roll()
    {
        CurrentValue = Random.Range(1, 7); // Dice roll between 1 and 6
    }
}