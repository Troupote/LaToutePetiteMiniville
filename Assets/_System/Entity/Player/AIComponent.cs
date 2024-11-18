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
        Debug.Log("AI is selecting a card...");

        List<CardSO> availableCards = piles.GetAvailableCards();

        if (availableCards.Count == 0)
        {
            Debug.LogWarning("No cards available for purchase.");
            yield break;
        }

        bool cardBought = false;

        while (!cardBought && availableCards.Count > 0)
        {
            card = availableCards[Random.Range(0, availableCards.Count)];
            Debug.Log($"AI selected card: {card.Name}");

            bool alreadyOwned = false;
            foreach (CardComponent cardComp in Cards)
            {
                if (cardComp.CardSO == card && cardComp.CardSO is BuildingSO building && building.IsUnique)
                {
                    Debug.LogWarning($"The card {card.Name} is already in the AI's hand and is unique.");
                    alreadyOwned = true;
                    break;
                }
            }

            if (alreadyOwned)
            {
                availableCards.Remove(card);
                continue;
            }

            if (Coins < card.Cost)
            {
                Debug.LogWarning($"AI cannot purchase the card: {card.Name}. Available: {Coins}, Required: {card.Cost}");
                availableCards.Remove(card);
                continue;
            }

            Add_SubstarctCoins(-card.Cost, null);

            if (piles.Piles.ContainsKey(card))
            {
                piles.Piles[card]--;
                if (piles.Piles[card] <= 0)
                {
                    piles.Piles.Remove(card);
                    Debug.Log($"Card {card.Name} is now out of stock.");
                }
            }

            _onBuyCard.Invoke(this, card);

            Debug.Log($"AI successfully purchased the card: {card.Name}");
            cardBought = true;
        }

        if (!cardBought)
            Debug.LogWarning("AI could not buy any cards.");

        yield return null;
    }

}
