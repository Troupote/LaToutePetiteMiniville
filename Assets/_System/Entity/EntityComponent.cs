using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using UnityEngine;
using System.Collections;
using System;

public abstract class EntityComponent : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private EntityConfigSO _config = null;

    [SerializeField]
    private TMP_Text _coinText = null;

    private string _name = string.Empty;
    private int _coins = 0;
    private List<CardComponent> _cards = new();
    private int _diceCount = 1;
    protected UnityEvent<EntityComponent, CardSO> _onBuyCard = new();
    #endregion

    #region Public API
    // Propri�t� avec 'protected set' pour permettre la modification seulement dans les classes d�riv�es
    public UnityEvent<EntityComponent, CardSO> OnBuyCard => _onBuyCard;

    public int DiceCount => Mathf.Max(1, _diceCount);
    public int Coins { get => Mathf.Max(0, _coins); protected set => _coins = value; } // Propri�t� avec 'protected set'
    public List<CardComponent> Cards { get => _cards; protected set => _cards = value; } // Propri�t� avec 'protected set'

    // M�thode pour ajouter ou soustraire des pi�ces
    public int Add_SubstarctCoins(int amount, Action onDone)
    {

        if (this is PlayerComponent)
            Debug.LogWarning("Player effects");
        if (this is AIComponent)
            Debug.LogWarning("AI effects");


        int value = Coins + amount;
        _coinText.text = $"{value}";
        Coins = value;

        if (onDone != null)
        {
            onDone.Invoke();
            Debug.Log("Add coin EFFECT");
        }
        return value;
    }

    public virtual IEnumerator ProcessBuyCard(Pile piles, CardSO card)
    {
        foreach (CardComponent cardComp in _cards)
        {
            if (cardComp == card && cardComp.CardSO is BuildingSO building && building.IsUnique)
                yield break;
        }

        if (_coins < card.Cost)
        {
            Debug.LogError($"Not enough coins to buy the card: {card.Name}");
            yield break;
        }
    }

    public void UnlockDice(Action onDone)
    {
        _diceCount++;
        _diceCount = Mathf.Clamp(_diceCount, 1, 2);

        Debug.Log("Dice count : " + _diceCount);
        Debug.Log("unlock dice EFFECT");
        onDone.Invoke();
    }

    public void Exchange(EntityComponent opponent)
    {
        Debug.Log("Exchange EFFECT");

    }

    #endregion

    #region Private API

    protected virtual void Init()
    {
        _coins = _config.Coins;
        _coinText.text = $"{_coins}";

        Transform parent = this is AIComponent ? FindFirstObjectByType<AIHand>().transform : FindFirstObjectByType<PlayerHand>().transform;

        foreach (CardSO card in _config.InitialCards)
        {
            CardComponent c = card.Build(transform.position, Quaternion.identity, parent);
            _cards.Add(c);
        }

        Debug.Log("Default cards : " + _cards.Count);
    }

    #endregion
}
