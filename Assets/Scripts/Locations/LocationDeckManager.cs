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
    public GameObject locationCardUIPrefab; // The prefab that has LocationCardUI

    [Header("Active Cards Limit")]
    [Range(1, 4)]
    public int maxActiveCards = 4;

    // Keep track of the active card UI instances
    private List<LocationCardUI> activeCardUIs = new List<LocationCardUI>();

    // Delegates/Events for other managers to listen
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
        // Fill up to maxActiveCards from the deck
        for (int i = 0; i < maxActiveCards; i++)
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

        // Create the UI for the next card
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

    /// <summary>
    /// Called (for example) at end of turn. Check each active card to see if it is fulfilled.
    /// If so, apply reward, remove card, spawn a new one.
    /// </summary>
    public void CheckCardResolutions(ResourceManager resourceManager, ResourceSO goldResource)
    {
        List<LocationCardUI> resolvedCards = new List<LocationCardUI>();

        foreach (var cardUI in activeCardUIs)
        {
            if (cardUI.IsCardFulfilled())
            {
                // Apply the reward
                if (resourceManager != null && goldResource != null)
                {
                    resourceManager.AddResource(goldResource, cardUI.cardData.goldReward);
                }

                // Notify via delegate/event
                OnCardResolved?.Invoke(cardUI.cardData);

                // Collect it for removal
                resolvedCards.Add(cardUI);
            }
        }

        // Remove resolved cards from the active list
        foreach (var resolvedCard in resolvedCards)
        {
            activeCardUIs.Remove(resolvedCard);
            Destroy(resolvedCard.gameObject);
        }

        // Spawn new cards to fill empty slots
        while (activeCardUIs.Count < maxActiveCards && deckIndex < locationDeck.Count)
        {
            SpawnNextCard();
        }
    }

    /// <summary>
    /// Called each turn if we want to apply ongoing effects (e.g. reduce resource).
    /// </summary>
    public void ApplyOngoingEffects(ResourceManager resourceManager, ResourceSO populationResource)
    {
        // Loop through active cards, apply effect if flagged
        foreach (var cardUI in activeCardUIs)
        {
            var data = cardUI.cardData;
            if (data.hasOngoingEffect && resourceManager != null && populationResource != null)
            {
                resourceManager.DeductResource(populationResource, data.ongoingEffectAmount);
            }
        }
    }
}
