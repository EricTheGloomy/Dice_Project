using UnityEngine;

[CreateAssetMenu(fileName = "NewDiceColor", menuName = "Dice/Dice Color")]
public class DiceColorSO : ScriptableObject
{
    public DiceColor ColorEnum; // Enum for safe referencing
    public string Name;       // Color name (e.g., "Red")
    public Color DisplayColor; // Unity Color for display
    public Sprite Icon;       // Optional: Icon to represent the color
}
