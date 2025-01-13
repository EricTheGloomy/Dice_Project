using UnityEngine;

[CreateAssetMenu(fileName = "StartingDiceConfig", menuName = "Dice/Starting Dice Configuration")]
public class StartingDiceSO : ScriptableObject
{
    public StartingDiceEntry[] startingDice; // Array to define dice configuration
}

[System.Serializable]
public class StartingDiceEntry
{
    public DiceColorSO color; // Reference to the color of the dice
    public int count;         // Number of dice to create with this color
}
