using UnityEngine;

[CreateAssetMenu(fileName = "StartingDiceConfig", menuName = "Dice/Starting Dice Configuration")]
public class StartingDiceSO : ScriptableObject
{
    public StartingDiceEntry[] startingDice;
}

[System.Serializable]
public class StartingDiceEntry
{
    public DiceColor colorEnum;
    public int count;
    public bool isPermanent;
}
