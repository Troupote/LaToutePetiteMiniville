using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    private CardSO _cardSO;
    private string _name;
    private int _coins;
    private List<CardSO> _cards;
    private int _nbDices;

    public int RollDices()
    {
        return Random.Range(1, 7);
    }

    public bool BuyCard(CardSO card)
    {
        if (_coins < card.Cost)
        {
            return false;
        } 
        _coins -= card.Cost;
        //TODO : add builder
        _cards.Add(card);
        return true;

    }

}
