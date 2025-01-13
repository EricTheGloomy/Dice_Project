using UnityEngine;

[CreateAssetMenu(fileName = "StartingDiceConfig", menuName = "Dice/Starting Dice Configuration")]
public class StartingDiceSO : ScriptableObject
{
    public StartingDiceEntry[] startingDice;
}

[System.Serializable]
public class StartingDiceEntry
{
    public DiceColor colorEnum; // Enum for safe referencing
    public int count;           // Number of dice to create with this color
    public bool isPermanent;
}
