using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIComponent : EntityComponent
{
    private void Start()
    {
        base.Init();
    }

    public override IEnumerator ProcessBuyCard(Pile piles, CardSO card)
    {
        Debug.Log($"AI is selecting a card...");

        // Retrieve the list of available cards from the piles
        List<CardSO> availableCards = piles.GetAvailableCards();

        // If the list of available cards is empty, exit the method
        if (availableCards.Count == 0)
        {
            Debug.Log("No cards available to buy.");
            yield break;
        }

        // Select a random card from the available cards
        card = availableCards[Random.Range(0, availableCards.Count)];
        Debug.Log($"AI selected card: {card.name}");

        // Check if the card is already in the AI's hand (especially for unique cards)
        foreach (CardComponent cardComp in Cards)
        {
            if (cardComp.CardSO == card && cardComp.CardSO is BuildingSO building && building.IsUnique)
            {
                Debug.Log($"The card {card.Name} is already in the AI's hand.");
                yield break;  // If the card is unique and already in hand, the purchase is canceled
            }
        }

        // Check if the AI has enough coins to buy the card
        if (Coins < card.Cost)
        {
            Debug.LogError($"AI does not have enough coins to buy the card: {card.Name}. Available: {Coins}, Required: {card.Cost}");
            yield break;  // If the AI doesn't have enough coins, the purchase is canceled
        }

        // Update the AI's coins
        Add_SubstarctCoins(-card.Cost, null);

        // Update the available card pile
        if (piles.Piles.ContainsKey(card))
        {
            piles.Piles[card]--;
            if (piles.Piles[card] <= 0)
                piles.Piles.Remove(card);  // Remove the card from the pile if it is depleted
        }

        // Notify that the AI has purchased the card
        _onBuyCard.Invoke(this, card);

        Debug.Log($"AI successfully purchased card: {card.Name}");

        yield return null;
    }
}
