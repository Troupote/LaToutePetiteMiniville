using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pile
{
    private List<CardSO> _allTurnPile = new();
    private List<CardSO> _opponentPile = new();
    private List<CardSO> _selfPile = new();
    private List<CardSO> _specialPile = new(); //@todo a voir plus tard si on fait ou pas
   
    public List<CardSO> AllTurnPile=> _allTurnPile;
    public List<CardSO> OpponentPile => _opponentPile;
    
    public List<CardSO> SelfPile => _selfPile;
    public List<CardSO> SpecialPile => _specialPile;

    public Pile(List<CardSO> allTurnPile, List<CardSO> opponentPile, List<CardSO> selfPile, List<CardSO> specialPile)
    {
        _allTurnPile = allTurnPile;
        _opponentPile = opponentPile;
        _selfPile = selfPile;
        _specialPile = specialPile;
    }

    public List<CardSO> PileSort(List<CardSO> list, CardActivationType type)
    {
        List<CardSO> sortedList = new();
        foreach(CardSO card in list)
        {

            if (card.ActivationType == type)
            {
               sortedList.Add(card);
            }
        }
        return sortedList;
    }
}
