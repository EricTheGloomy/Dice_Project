using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour, IManager
{
    [Header("Skill Data")]
    [Tooltip("All skills the player starts with, or we can get them from a 'StartingSkillsSO' if you prefer.")]
    public StartingSkillsSO startingSkillsConfig;

    [Header("UI Setup")]
    [Tooltip("The parent container (UI transform) where we place the skill prefabs.")]
    public Transform skillContainer;
    public GameObject skillUIPrefab; // The prefab that has SkillUI

    private List<SkillUI> activeSkills = new List<SkillUI>();

    // References to other managers
    private DicePoolManager dicePoolManager;

    public void Initialize(GameController controller)
    {
        dicePoolManager = controller.dicePoolManager;
        Debug.Log("SkillManager initialized.");

        // For now, spawn all starting skills immediately
        SpawnStartingSkills();
    }

    private void SpawnStartingSkills()
    {
        if (startingSkillsConfig == null || startingSkillsConfig.startingSkills.Length == 0)
            return;

        foreach (var skillData in startingSkillsConfig.startingSkills)
        {
            SpawnSkill(skillData);
        }
    }

    /// <summary>
    /// Creates a new skill UI from the given skill data, and tracks it in activeSkills.
    /// </summary>
    public void SpawnSkill(SkillSO skillData)
    {
        if (skillUIPrefab == null || skillContainer == null)
        {
            Debug.LogWarning("SkillUIPrefab or SkillContainer not assigned in SkillManager.");
            return;
        }

        GameObject skillObj = Instantiate(skillUIPrefab, skillContainer);
        SkillUI skillUI = skillObj.GetComponent<SkillUI>();
        if (skillUI != null)
        {
            skillUI.Initialize(skillData);
            activeSkills.Add(skillUI);
        }
    }


    /// <summary>
    /// Applies the effect described by the SkillSO to the dice in the pool.
    /// </summary>
    public void ApplySkillEffect(SkillSO skillData)
    {
        // If the skill spawns a new dice, do it
        if (skillData.createNewDice && skillData.newDiceColor != null)
        {
            dicePoolManager.AddDiceWithFaceValue(
                skillData.newDiceColor, 
                false, // non-permanent
                skillData.newDiceFaceValue
            );
            return; // skip pipChange
        }

        // Otherwise, we manipulate existing dice in the pool
        if (skillData.pipChange != 0)
        {
            // Use the "ModifyPips" method from DicePoolManager
            // We'll pass a filter that returns true for dice we want to affect

            dicePoolManager.ModifyPips((dice) =>
            {
                // Only affect dice that are currently not assigned to any slot
                if (dice.IsAssignedToSlot) return false;

                // If the skill affects all dice, that's easy
                if (skillData.affectAllDice) return true;

                // If we only affect certain color
                if (skillData.affectOnlyColor)
                {
                    if (dice.LogicalColor != skillData.colorToAffect) return false;
                }

                // If only odds or evens
                if (skillData.affectOnlyOdds && (dice.CurrentValue % 2 == 0)) return false;
                if (skillData.affectOnlyEvens && (dice.CurrentValue % 2 == 1)) return false;

                // If we got here, it means the dice meets the filters
                return true;

            }, skillData.pipChange);
        }
    }

    public void ClearAllSkillSlots()
    {
        // For each skill UI, forcibly clear the slots
        foreach (SkillUI skill in activeSkills)
        {
            skill.ForceClearSlots();
        }
    }

}
