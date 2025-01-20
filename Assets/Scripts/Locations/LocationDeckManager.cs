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
        List<LocationCardUI> resolvedCards = new List<LocationCardUI>();

        foreach (var cardUI in activeCardUIs)
        {
            if (cardUI.IsCardFulfilled())
            {
                if (resourceManager != null && goldResource != null)
                {
                    resourceManager.AddResource(goldResource, cardUI.cardData.goldReward);
                }

                OnCardResolved?.Invoke(cardUI.cardData);

                resolvedCards.Add(cardUI);
            }
        }

        foreach (var resolvedCard in resolvedCards)
        {
            activeCardUIs.Remove(resolvedCard);
            Destroy(resolvedCard.gameObject);
        }

        while (activeCardUIs.Count < maxActiveCards && deckIndex < locationDeck.Count)
        {
            SpawnNextCard();
        }
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
}
