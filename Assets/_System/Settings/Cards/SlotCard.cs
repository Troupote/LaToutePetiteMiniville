using UnityEngine;
using UnityEngine.UI;

public class SlotCard : MonoBehaviour
{
    [SerializeField]
    private CardSO _card = null;

    [SerializeField]
    private  GameRunner gameRunner = null;

    private EntityComponent _entity;


    public void AddCardToPlayer()
    {
        foreach(var card in _entity.Cards)
        {
            if(card.CardSO == _card &&(_card is BuildingSO building && building.IsUnique))
             return;
            
            gameRunner.TryBuyCard(_card);

        }
    }
}
