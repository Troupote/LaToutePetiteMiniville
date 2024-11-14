using System.Collections.Generic;

/// <summary>
/// Initialize and Manage piles.
/// </summary>
public class Pile
{
    #region Fields

    /// <summary>
    /// The type of card for a piles.
    /// </summary>
    private CardSO[] _cardArchetypes = null;

    /// <summary>
    /// The whole piles with drawable cards.
    /// </summary>
    Dictionary<CardSO, int> _piles = new Dictionary<CardSO, int>();

    /// <summary>
    /// The stack size of a initialized pile.
    /// </summary>
    private int _baseCardStackSize = 0;

    #endregion

    #region Lifecycle

    /// <inheritdoc cref=" Pile"/>
    /// <param name="archetypes"><inheritdoc cref="_cardArchetypes"/></param>
    /// <param name="stackSize"><inheritdoc cref="_baseCardStackSize"/></param>
    public Pile(CardSO[] archetypes, int stackSize)
    {
        _cardArchetypes = archetypes;
        _baseCardStackSize = stackSize;

        InitPiles();
    }

    #endregion

    #region Public API

    ///<inheritdoc cref="_piles"/>
    public Dictionary<CardSO, int> Piles => _piles;

    private void InitPiles()
    {
        foreach (CardSO archetype in _cardArchetypes)
        {
            if (_piles.ContainsKey(archetype))
                continue;

            _piles.Add(archetype, _baseCardStackSize);
        }
    }

    private bool DrawCard(CardSO card)
    {
        if (_piles[card] <= 0)
            return false;

        _piles[card]--;
        return true;
    }

    #endregion
}
