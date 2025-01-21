using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class SkillSO : ScriptableObject
{
    [Header("Skill Info")]
    public string skillName;
    [TextArea] public string skillDescription;

    [Header("Dice Slots")]
    [Range(1, 3)]
    public int diceSlotCount = 1;

    // Here is where we store the restrictions for each slot
    public DiceSlotRestrictionSO[] slotRestrictions;

    [Header("Skill Effect Settings")]
    public bool createNewDice;
    public DiceColorSO newDiceColor;
    public int newDiceFaceValue;

    public bool affectAllDice;
    public bool affectOnlyColor;
    public DiceColorSO colorToAffect;
    public bool affectOnlyOdds;
    public bool affectOnlyEvens;

    public int pipChange;
}
