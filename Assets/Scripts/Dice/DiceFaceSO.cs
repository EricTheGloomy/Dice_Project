using UnityEngine;

[CreateAssetMenu(fileName = "NewDiceFace", menuName = "Dice/Dice Face")]
public class DiceFaceSO : ScriptableObject
{
    public int Value;       // Value of the dice face (e.g., 1–6)
    public Sprite Sprite;   // Sprite for this face
    public string Effect;   // Optional: Custom effect or description
}
