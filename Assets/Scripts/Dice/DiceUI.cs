using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DiceUI links the UI representation of the dice (face image, color, etc.)
/// to the underlying Dice data class.
/// </summary>
public class DiceUI : MonoBehaviour
{
    // Assign this in the prefab or via code
    public Image faceImage;
    
    // A reference back to the "Dice" data class
    public Dice dataReference;
}
