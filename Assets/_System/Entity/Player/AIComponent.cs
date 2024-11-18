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

        List<CardSO> availableCards = piles.GetAvailableCards();

        if (availableCards.Count == 0)
        {
            Debug.Log("No cards available to buy.");
            yield break;
        }

        card = availableCards[Random.Range(0, availableCards.Count)];
        Debug.Log($"AI selected card: {card.name}");

        foreach (CardComponent cardComp in Cards)
        {
            if (cardComp.CardSO == card && cardComp.CardSO is BuildingSO building && building.IsUnique)
            {
                Debug.Log($"The card {card.Name} is already in the AI's hand.");
                yield break;
            }
        }

        if (Coins < card.Cost)
        {
            Debug.LogError($"AI does not have enough coins to buy the card: {card.Name}. Available: {Coins}, Required: {card.Cost}");
            yield break; 
        }

        Add_SubstarctCoins(-card.Cost, null);

        if (piles.Piles.ContainsKey(card))
        {
            piles.Piles[card]--;
            if (piles.Piles[card] <= 0)
                piles.Piles.Remove(card);  
        }

        _onBuyCard.Invoke(this, card);

        Debug.Log($"AI successfully purchased card: {card.Name}");

        yield return null;
    }
}
