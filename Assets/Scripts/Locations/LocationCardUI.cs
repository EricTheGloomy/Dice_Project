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

    // The "Model" reference
    public LocationCardSO cardData; 

    // Keep references to the spawned slot objects
    private List<DiceSlot> spawnedSlots = new List<DiceSlot>();

    /// <summary>
    /// Called by LocationDeckManager (or some controller) to set up this UI.
    /// </summary>
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

        // Spawn dice slots based on the data
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
                // Assign the restriction from the array if it exists
                if (data.slotRestrictions != null && i < data.slotRestrictions.Length)
                {
                    slot.slotRestriction = data.slotRestrictions[i];
                }

                spawnedSlots.Add(slot);
            }
        }
    }

    /// <summary>
    /// This method can be called externally to update the "visible side" of the card 
    /// if you want to show back or front, or you can do it automatically.
    /// </summary>
    public void ShowFront(bool show)
    {
        if (frontImage != null) frontImage.gameObject.SetActive(show);
        if (backImage != null) backImage.gameObject.SetActive(!show);
    }

    /// <summary>
    /// Check if all dice slots are fulfilled, meaning the card is 'resolved'.
    /// </summary>
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

    /// <summary>
    /// Access spawned DiceSlots if needed externally (like for cleanup or checks).
    /// </summary>
    public List<DiceSlot> GetSlots()
    {
        return spawnedSlots;
    }
}
