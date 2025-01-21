using UnityEngine;
using TMPro;
using UnityEngine.UI; // for Button
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
                // or just set useSkillButton.interactable = false;
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

//TO DO should be in skill manager probably
    public void UseSkill()
    {
        // If you have a reference to your SkillManager or GameController:
        SkillManager skillManager = FindObjectOfType<SkillManager>();
        if (skillManager != null)
        {
            skillManager.ApplySkillEffect(skillData);
        }

        // Reset the dice slots
        foreach (var slot in spawnedSlots)
        {
            slot.ClearSlot();
            slot.UnfulfillSlotVisuals();
        }

        // Hide the button
        if (useSkillButton != null)
        {
            useSkillButton.gameObject.SetActive(false);
        }

        Debug.Log($"Skill {skillData.skillName} used, effect applied!");
    }

}
