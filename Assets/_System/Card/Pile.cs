using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Initialize and Manage piles.
/// </summary>
public class Pile
{
    #region Fields

    /// <summary>
    /// The type of card for a pile.
    /// </summary>
    private CardSO[] _cardArchetypes = null;

    /// <summary>
    /// The whole pile with drawable cards.
    /// </summary>
    private Dictionary<CardSO, int> _piles = new Dictionary<CardSO, int>();

    /// <summary>
    /// The stack size of an initialized pile.
    /// </summary>
    private int _baseCardStackSize = 0;

    #endregion

    #region Lifecycle

    /// <inheritdoc cref="Pile"/>
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

    /// <summary>
    /// Initialize the pile with the base card stack size.
    /// </summary>
    private void InitPiles()
    {
        foreach (CardSO archetype in _cardArchetypes)
        {
            if (_piles.ContainsKey(archetype))
                continue;

            _piles.Add(archetype, _baseCardStackSize);
        }
    }

    /// <summary>
    /// Draws a card from the pile, reducing its count.
    /// </summary>
    /// <param name="card">The card to draw.</param>
    /// <returns>True if the card was successfully drawn, false otherwise.</returns>
    public bool DrawCard(CardSO card)
    {
        if (!_piles.ContainsKey(card) || _piles[card] <= 0)
            return false;

        _piles[card]--;
        return true;
    }

    /// <summary>
    /// Returns all cards with at least one instance available in the pile.
    /// </summary>
    /// <returns>List of available cards.</returns>
    public List<CardSO> GetAvailableCards() => _piles
        .Where(d => d.Value > 0)
        .Select(d => d.Key)
        .ToList();

    #endregion
}
