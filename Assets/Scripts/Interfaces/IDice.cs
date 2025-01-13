using UnityEngine;

public interface IDice
{
    int CurrentValue { get; } // The current value of the dice.
    DiceColorSO LogicalColor { get; } // The color and theme of the dice.
    bool IsPermanent { get; } //Is the dice permanent
    Sprite CurrentSprite { get; } // The sprite representing the current face.
}
