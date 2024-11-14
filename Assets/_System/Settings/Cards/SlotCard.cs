using UnityEngine;
using UnityEngine.UI;

public class SlotCard : MonoBehaviour
{
    [SerializeField]
    private CardSO _card = null;

    [SerializeField]
    private  GameRunner gameRunner = null;

    private EntityComponent _playerCard;


    public void AddCardToPlayer()
    {
        foreach(var card in _playerCard.Cards)
        {
            if(card.CardSO.name == _card.name && card.CardSO.ActivationType == CardActivationType.SelfTurn && _card.ActivationType == CardActivationType.SelfTurn)
             return;
            
            gameRunner.BuyCard(_card);

        }
    }
}
