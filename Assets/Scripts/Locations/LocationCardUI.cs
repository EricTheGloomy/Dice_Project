using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// The UI 'View' that displays a location card and its dice slots.
/// </summary>
public class LocationCardUI : MonoBehaviour
{
    [Header("Card Visuals")]
    public Image frontImage; 
    public Image backImage;
    public TMP_Text locationNameText;
    public TMP_Text descriptionText;

    [Header("Dice Slot Parent")]
    public Transform diceSlotsContainer; 
    public GameObject diceSlotPrefab; // A UI prefab for an individual slot

    public LocationCardSO cardData; 

    private List<DiceSlot> spawnedSlots = new List<DiceSlot>();


    public void Initialize(LocationCardSO data)
    {
        cardData = data;

        if (frontImage != null && data.frontSprite != null)
            frontImage.sprite = data.frontSprite;

        if (backImage != null && data.backSprite != null)
            backImage.sprite = data.backSprite;

        if (locationNameText != null)
            locationNameText.text = data.locationName;

        if (descriptionText != null)
            descriptionText.text = data.description;

        SpawnDiceSlots(data);
    }

    private void SpawnDiceSlots(LocationCardSO data)
    {
        // Clear any existing slots from a previous setup
        foreach (var slot in spawnedSlots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }
        spawnedSlots.Clear();

        // Safety check: If the user sets # of dice slots but doesn't fill the array
        // we might need to handle that
        int slotCount = data.numberOfDiceSlots;

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(diceSlotPrefab, diceSlotsContainer);
            DiceSlot slot = slotObj.GetComponent<DiceSlot>();
            if (slot != null)
            {
                if (data.slotRestrictions != null && i < data.slotRestrictions.Length)
                {
                    slot.SetRestriction(data.slotRestrictions[i]);
                }
                else
                {
                    slot.SetRestriction(null);
                }

                spawnedSlots.Add(slot);
            }
        }
    }

    public void ShowFront(bool show)
    {
        if (frontImage != null) frontImage.gameObject.SetActive(show);
        if (backImage != null) backImage.gameObject.SetActive(!show);
    }

    public bool IsCardFulfilled()
    {
        foreach (var slot in spawnedSlots)
        {
            if (slot != null && !slot.isRequirementFulfilled)
            {
                return false;
            }
        }
        return true;
    }

    public List<DiceSlot> GetSlots()
    {
        return spawnedSlots;
    }
}
