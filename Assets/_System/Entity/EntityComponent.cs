using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// The base class representing an entity in runtime.
/// </summary>
public abstract class EntityComponent : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private EntityConfigSO _config = null;

    [SerializeField]
    private TMP_Text _coinText = null;

    /// <summary>
    /// The entity debug name.
    /// </summary>
    private string _name = string.Empty;

    /// <summary>
    /// The amount of coins the entiy has.
    /// </summary>
    private int _coins = 0;

    /// <summary>
    /// The list of <see cref="CardComponent"/>s the entity has.
    /// </summary>
    private List<CardComponent> _cards = new();

    private int _diceCount = 0;

    #endregion

    #region Public API

    public int DiceCount => Mathf.Max(1, _diceCount);
    ///<inheritdoc cref="_coins"/>
    public int Coins => Mathf.Max(0, _coins);

    ///<inheritdoc cref="_cards"/>
    public List<CardComponent> Cards => _cards;


    /// <summary>
    /// Buy a card and add it in hand.
    /// </summary>
    /// <param name="cardSO"></param>
    /// <returns>Returns true if a card has been purchased succesfully.</returns>
    public bool BuyCard(CardSO cardSO)
    {
        if (_coins < cardSO.Cost)
            return false;

        _coins -= cardSO.Cost;

        _coinText.text = $"{_coins}";


        return true;
    }

    /// <summary>
    /// Add an amount of coins to an entity.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>returns the new coins value.</returns>
    public int IncrementCoins(int amount)
    {
        int value = _coins + amount;
        _coinText.text = $"{value}";

        return value;
    }

    public bool Exchange(EntityComponent opp)
    {
        Debug.Log("Exchange");

        return true;
    }

    #endregion

    #region Private API

    /// <summary>
    /// Initialize this entity using <see cref="EntityConfigSO"/>
    /// </summary>
    protected virtual void Init()
    {
        _coins = _config.Coins;

        _coinText.text = $"{_coins}";
    }


    public bool UnlockDice()
    {
        if (_diceCount >= 2)
            return false;

        _diceCount++;
        return true;
    }


    #endregion
}
