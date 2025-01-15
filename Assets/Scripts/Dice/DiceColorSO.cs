using UnityEngine;

[CreateAssetMenu(fileName = "NewDiceColor", menuName = "Dice/Dice Color")]
public class DiceColorSO : ScriptableObject
{
    public DiceColor ColorEnum; // Enum for safe referencing
    public string Name;
    public Color DisplayColor;
    public Sprite Icon;       // Optional: Icon to represent the color
}
