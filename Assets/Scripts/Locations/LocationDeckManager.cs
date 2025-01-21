using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a deck of Location Cards and spawns up to 4 active cards.
/// </summary>
public class LocationDeckManager : MonoBehaviour, IManager
{
    [Header("Deck Setup")]
    [Tooltip("All location cards available in the game. Could be shuffled or used in order.")]
    public List<LocationCardSO> locationDeck;

    [Header("UI Setup")]
    [Tooltip("Parent transform for the 4 card slots in the UI.")]
    public Transform cardContainer; 
    public GameObject locationCardUIPrefab;

    [Header("Active Cards Limit")]
    [Range(1, 4)]
    public int maxActiveCards = 4;

    [Header("Stage Settings")]
    public bool useStages = true; // If you want to turn on/off stage-based flow
    public int currentStage = 1;
    public int cardsPerStage = 4; // how many cards appear per stage

    private List<LocationCardUI> activeCardUIs = new List<LocationCardUI>();

    public delegate void CardResolvedHandler(LocationCardSO cardData);
    public event CardResolvedHandler OnCardResolved;

    // For deck indexing or shuffling
    private int deckIndex = 0;

    public void Initialize(GameController controller)
    {
        Debug.Log("LocationDeckManager initialized.");
        // For a real game, you might want to shuffle your deck here.
        // For now, do nothing.
    }

    public void PopulateInitialCards()
    {
        // If you're using stage-based flow, we only spawn `cardsPerStage` for the first stage:
        // Otherwise, if not using stage flow, we can do the old approach of spawning maxActiveCards.
        if (useStages)
        {
            SpawnStage(currentStage);
        }
        else
        {
            // old approach
            for (int i = 0; i < maxActiveCards; i++)
            {
                if (deckIndex < locationDeck.Count)
                {
                    SpawnNextCard();
                }
            }
        }
    }

    // This spawns 4 new cards (or however many you define per stage)
    private void SpawnStage(int stageNumber)
    {
        // Clear out any existing cards first if you want them gone
        // (or if you want them to remain on screen, skip this)
        ClearCurrentCards();

        // Spawn the next set of cards from the deck
        for (int i = 0; i < cardsPerStage; i++)
        {
            if (deckIndex < locationDeck.Count)
            {
                SpawnNextCard();
            }
        }
    }
    private void SpawnNextCard()
    {
        if (deckIndex >= locationDeck.Count)
        {
            Debug.LogWarning("No more cards in the deck!");
            return;
        }

        var nextCardData = locationDeck[deckIndex];
        var cardObj = Instantiate(locationCardUIPrefab, cardContainer);
        var cardUI = cardObj.GetComponent<LocationCardUI>();

        if (cardUI != null)
        {
            cardUI.Initialize(nextCardData);
            // Show the front side immediately, or show the back if you want it facedown
            cardUI.ShowFront(true);

            activeCardUIs.Add(cardUI);
        }

        deckIndex++;
    }

    public void CheckCardResolutions(ResourceManager resourceManager, ResourceSO goldResource)
    {
        // Reward or flip each card if fully resolved:
        foreach (var cardUI in activeCardUIs)
        {
            if (cardUI.IsCardFulfilled())
            {
                // reward, flip, etc.
                if (resourceManager != null && goldResource != null)
                {
                    resourceManager.AddResource(goldResource, cardUI.cardData.goldReward);
                }
                OnCardResolved?.Invoke(cardUI.cardData);

                cardUI.ShowFront(false);
            }
        }

        // Now check if *all* are resolved
        bool allResolved = true;
        foreach (var cardUI in activeCardUIs)
        {
            if (!cardUI.IsCardFulfilled())
            {
                allResolved = false;
                break;
            }
        }

        // If stage-based flow is on, and indeed all are resolved...
        if (useStages && allResolved)
        {
            Debug.Log($"Stage {currentStage} all resolved. Moving to next stage!");
            currentStage++;
            SpawnStage(currentStage);
        }
    }

    // If you want to remove the old cards from the screen
    // before spawning the next stage:
    private void ClearCurrentCards()
    {
        foreach (var cardUI in activeCardUIs)
        {
            Destroy(cardUI.gameObject);
        }
        activeCardUIs.Clear();
    }

    public void ApplyOngoingEffects(ResourceManager resourceManager, ResourceSO populationResource)
    {
        foreach (var cardUI in activeCardUIs)
        {
            var data = cardUI.cardData;
            if (data.hasOngoingEffect && resourceManager != null && populationResource != null)
            {
                resourceManager.DeductResource(populationResource, data.ongoingEffectAmount);
            }
        }
    }

    public void CheckAllSlotsOnActiveCards()
    {
        // For each card in the active list
        foreach (var cardUI in activeCardUIs)
        {
            // For each slot in that card
            foreach (var slot in cardUI.GetSlots())
            {
                // If the slot is already fulfilled, skip it
                if (slot.isRequirementFulfilled)
                    continue;

                bool requirementMet = false;
                // Check if there's a dice currently assigned
                var diceGO = slot.GetCurrentDice();
                if (diceGO != null)
                {
                    var diceUI = diceGO.GetComponent<DiceUI>();
                    if (diceUI != null && diceUI.dataReference != null)
                    {
                        // Use the slot's own restriction to check if the dice meets the requirement
                        requirementMet = slot.CanAcceptDice(diceUI.dataReference);
                    }
                }

                // If requirement is met now, mark the slot as fulfilled (visually + logically)
                if (requirementMet)
                {
                    slot.FulfillSlotVisuals();
                }
                else
                {
                    // If it's not met, ensure the slot visuals are not showing "fulfilled"
                    slot.UnfulfillSlotVisuals();
                }
            }
        }
    }

}
