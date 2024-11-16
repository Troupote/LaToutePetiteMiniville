using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class representing an entity in runtime.
/// </summary>
public abstract class EntityComponent : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private EntityConfigSO _config = null;

    [SerializeField]
    private SlotPlayerCard _toCreateAnObject;
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

    public int diceCount = 2;

    #endregion

    #region Public API

    ///<inheritdoc cref="_coins"/>
    public int Coins => Mathf.Max(0, _coins);

    ///<inheritdoc cref="_cards"/>
    public List<CardComponent> Cards => _cards;


    /// <summary>
    /// Roll the dice.
    /// </summary>
    /// <returns></returns>
    public int RollDices() => Random.Range(1, 7); //@todo check usefull ?

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

        //CardComponent card = cardSO.Build();
        //_cards.Add(card);
        _toCreateAnObject.AddCarteOnScene();

        return true;
    }

    /// <summary>
    /// Add an amount of coins to an entity.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>returns the new coins value.</returns>
    public int IncrementCoins(int amount) => _coins += amount;

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
    }
    

    public bool unlockDice()
    {
        if ( diceCount < 2)
        {
            diceCount++;
            return true;
        }
        return false;
    }

    #endregion
}
