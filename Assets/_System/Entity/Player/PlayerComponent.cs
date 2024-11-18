using System.Collections;
using UnityEngine;

public class PlayerComponent : EntityComponent
{
    private void Start()
    {
        base.Init();
    }

    public override IEnumerator ProcessBuyCard(Pile piles, CardSO card)
    {
        foreach (CardComponent cardComp in Cards)
        {
            if (cardComp.CardSO == card && cardComp.CardSO is BuildingSO building && building.IsUnique)
            {
                Debug.Log($"The card {card.Name} is unique");
                yield break;
            }
        }

        if (Coins < card.Cost)
        {
            Debug.LogWarning($"Player cannot purchase the card: {card.Name}. Available: {Coins}, Required: {card.Cost}");

            yield break;
        }

        Add_SubstarctCoins(-card.Cost, null);

        if (piles.Piles.ContainsKey(card))
        {
            piles.Piles[card]--;
            if (piles.Piles[card] <= 0)
                piles.Piles.Remove(card);
        }

        Debug.Log($"Achat réussi! Carte {card.Name} ajoutée à votre main.");
        _onBuyCard.Invoke(this, card);

        yield return null;
    }
}
