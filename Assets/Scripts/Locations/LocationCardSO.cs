using UnityEngine;

/// <summary>
/// Data class for describing a location card.
/// </summary>
[CreateAssetMenu(fileName = "NewLocationCard", menuName = "Cards/Location Card")]
public class LocationCardSO : ScriptableObject
{
    [Header("Basic Info")]
    public string locationName;
    [TextArea] public string description;

    [Header("Art")]
    public Sprite frontSprite;   // The front side image
    public Sprite backSprite;    // The back side image (if needed)

    [Header("Reward")]
    public int goldReward;       // Simple example: awarding gold

    [Header("Ongoing Effect")]
    [Tooltip("If true, an ongoing effect triggers each turn. For example, 'Decrease population resource by 1'.")]
    public bool hasOngoingEffect;
    public int ongoingEffectAmount; // The amount to reduce population (or some other resource) each turn

    [Header("Dice Slots Required")]
    [Range(1, 6)]
    public int numberOfDiceSlots;
    [Tooltip("Restrictions for each dice slot. Keep the array size in sync with numberOfDiceSlots.")]
    public DiceSlotRestrictionSO[] slotRestrictions;
}
