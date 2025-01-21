using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text skillNameText;
    public TMP_Text skillDescriptionText;

    [Header("Slot Container")]
    public Transform diceSlotsContainer;
    public GameObject diceSlotPrefab;

    [Header("Use Skill Button")]
    public Button useSkillButton;

    // The "model" data
    [HideInInspector]
    public SkillSO skillData;

    private List<DiceSlot> spawnedSlots = new List<DiceSlot>();

    public void Initialize(SkillSO data)
    {
        skillData = data;

        // Basic text
        if (skillNameText != null) 
            skillNameText.text = data.skillName;
        if (skillDescriptionText != null)
            skillDescriptionText.text = data.skillDescription;

        SpawnDiceSlots();
        
        // Initially, the button is disabled if not all slots are fulfilled
        if (useSkillButton != null)
        {
            useSkillButton.onClick.AddListener(UseSkill);
            useSkillButton.gameObject.SetActive(false); 
        }
    }

    private void SpawnDiceSlots()
    {
        // Clear old slots if any
        foreach (var slot in spawnedSlots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }
        spawnedSlots.Clear();

        int slotCount = skillData.diceSlotCount;
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(diceSlotPrefab, diceSlotsContainer);
            DiceSlot slot = slotObj.GetComponent<DiceSlot>();

            if (slot != null)
            {
                // If skillData has an array of slotRestrictions,
                // and we have one for this slot, assign it:
                if (skillData.slotRestrictions != null && i < skillData.slotRestrictions.Length)
                {
                    slot.SetRestriction(skillData.slotRestrictions[i]);
                }
                // else leave it unrestricted

                spawnedSlots.Add(slot);
            }
        }
    }

    // Called each frame to check if the skill can be used
    private void Update()
    {
        if (AllSlotsFulfilled())
        {
            // Either enable the button, or show it
            if (useSkillButton != null)
            {
                useSkillButton.gameObject.SetActive(true);
            }
        }
        else
        {
            // Hide or disable it
            if (useSkillButton != null) 
            {
                useSkillButton.gameObject.SetActive(false);
            }
        }
    }

    private bool AllSlotsFulfilled()
    {
        // Check each slot
        foreach (var slot in spawnedSlots)
        {
            if (!slot.isRequirementFulfilled)
            {
                return false;
            }
        }
        return true;
    }

    public void UseSkill()
    {
        // 1) Apply the skill effect
        SkillManager skillManager = FindObjectOfType<SkillManager>();
        if (skillManager != null)
        {
            skillManager.ApplySkillEffect(skillData);
        }

        // 2) Mark all dice used in these slots as used-for-the-turn
        foreach (var slot in spawnedSlots)
        {
            var diceGO = slot.GetCurrentDice();
            if (diceGO != null)
            {
                DiceUI diceUI = diceGO.GetComponent<DiceUI>();
                if (diceUI != null && diceUI.dataReference != null)
                {
                    // Mark this dice as used
                    diceUI.dataReference.IsUsedThisTurn = true;

                    // Optionally hide the dice so it can't be dragged out again
                    diceGO.SetActive(false);
                }
            }
        }

        // 3) Clear the slots so the skill can be reused with fresh dice
        //    (if you want the skill to be reusable multiple times in a single turn)
        foreach (var slot in spawnedSlots)
        {
            slot.ClearSlot();
            slot.UnfulfillSlotVisuals();
        }

        // 4) Hide the button
        if (useSkillButton != null)
        {
            useSkillButton.gameObject.SetActive(false);
        }

        Debug.Log($"Skill {skillData.skillName} used, effect applied!");
    }

    public void ForceClearSlots()
    {
        foreach (DiceSlot slot in spawnedSlots)
        {
            // If there's a dice assigned, let's unassign it
            slot.ClearSlot();
            slot.isRequirementFulfilled = false;
            slot.UnfulfillSlotVisuals();
        }

        // If we also want to hide the "Use Skill" button
        if (useSkillButton != null)
        {
            useSkillButton.gameObject.SetActive(false);
        }
    }

}
