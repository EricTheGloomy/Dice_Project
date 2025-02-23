using UnityEngine;

public class Dice : IDice
{
    public DiceColorSO LogicalColor { get; private set; }
    public int CurrentValue { get; set; } // State managed by DiceManager
    public Sprite CurrentSprite => diceFaces[CurrentValue - 1].Sprite;

    public GameObject UIContainerObject { get; set; } // Reference to UI element

    public bool IsPermanent { get; private set; }

    public bool IsAssignedToSlot { get; set; }

    // NEW: Mark this dice as used for this turn, preventing re-roll
    public bool IsUsedThisTurn { get; set; }

    private DiceFaceSO[] diceFaces;

    public Dice(DiceColorSO color, DiceFaceSO[] faces, bool isPermanent = false)
    {
        LogicalColor = color;
        diceFaces = faces;
        CurrentValue = Random.Range(1, diceFaces.Length + 1);
        IsPermanent = isPermanent;
        IsAssignedToSlot = false;

        // Defaults
        IsUsedThisTurn = false;
    }
}
