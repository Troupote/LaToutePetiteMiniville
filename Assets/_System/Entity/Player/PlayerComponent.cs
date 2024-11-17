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
                Debug.Log($"La carte {card.Name} est déjà dans votre main.");
                yield break;  
            }
        }

        if (Coins < card.Cost)
        {
            Debug.LogError($"Pas assez de pièces pour acheter la carte: {card.Name}");
            yield break;
        }

        Add_SubstarctCoins(-card.Cost);

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
