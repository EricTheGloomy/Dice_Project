using UnityEngine;

[CreateAssetMenu(fileName = "NewDiceColor", menuName = "Dice/Dice Color")]
public class DiceColorSO : ScriptableObject
{
    public string Name;       // Color name (e.g., "Red")
    public Color DisplayColor; // Unity Color for display
    public Sprite Icon;       // Optional: Icon to represent the color
}
