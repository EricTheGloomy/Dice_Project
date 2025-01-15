using UnityEngine;

public interface IDice
{
    int CurrentValue { get; }
    DiceColorSO LogicalColor { get; }
    bool IsPermanent { get; }
    bool IsAssignedToSlot { get; }
    Sprite CurrentSprite { get; }
}
