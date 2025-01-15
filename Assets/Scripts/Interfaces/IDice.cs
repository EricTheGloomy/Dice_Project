using UnityEngine;

public interface IDice
{
    int CurrentValue { get; }
    DiceColorSO LogicalColor { get; }
    bool IsPermanent { get; }
    Sprite CurrentSprite { get; }
}
