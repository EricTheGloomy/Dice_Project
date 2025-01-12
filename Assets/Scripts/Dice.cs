using UnityEngine;

public class Dice
{
    public DiceColorSO LogicalColor { get; private set; }
    public int CurrentValue { get; set; } // State managed by DiceManager
    public Sprite CurrentSprite => diceFaces[CurrentValue - 1].Sprite;

    public GameObject UIContainerObject { get; set; } // Reference to UI element

    private DiceFaceSO[] diceFaces;

    public Dice(DiceColorSO color, DiceFaceSO[] faces)
    {
        LogicalColor = color;
        diceFaces = faces;
        CurrentValue = 1; // Default value
    }
}
