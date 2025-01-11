using UnityEngine;

public class Dice
{
    public DiceColorSO LogicalColor { get; private set; }
    public int CurrentValue { get; private set; }
    public Sprite CurrentSprite => diceFaces[CurrentValue - 1].Sprite;

    public GameObject UIContainerObject { get; set; } // Reference to UI element

    private DiceFaceSO[] diceFaces;

    public Dice(DiceColorSO color, DiceFaceSO[] faces)
    {
        LogicalColor = color;
        CurrentValue = 1; // Default to 1
        diceFaces = faces;
    }

    public void Roll()
    {
        CurrentValue = Random.Range(1, diceFaces.Length + 1); // Roll between 1 and max faces
    }

    public void SubtractPip()
    {
        CurrentValue = Mathf.Max(1, CurrentValue - 1); // Ensure it doesn’t go below 1
    }

    public void AddPip()
    {
        CurrentValue = Mathf.Min(diceFaces.Length, CurrentValue + 1); // Ensure it doesn’t exceed max faces
    }
}
