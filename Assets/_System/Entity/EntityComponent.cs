using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using UnityEngine;
using System.Collections;

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
    private int _diceCount = 0;
    protected UnityEvent<EntityComponent, CardSO> _onBuyCard = new();
    #endregion

    #region Public API
    // Propriété avec 'protected set' pour permettre la modification seulement dans les classes dérivées
    public UnityEvent<EntityComponent, CardSO> OnBuyCard => _onBuyCard;

    public int DiceCount => Mathf.Max(1, _diceCount);
    public int Coins { get => Mathf.Max(0, _coins); protected set => _coins = value; } // Propriété avec 'protected set'
    public List<CardComponent> Cards { get => _cards; protected set => _cards = value; } // Propriété avec 'protected set'

    // Méthode pour ajouter ou soustraire des pièces
    public int Add_SubstarctCoins(int amount)
    {
        int value = _coins + amount;
        _coinText.text = $"{value}";
        _coins = value;
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

    public void UnlockDice()
    {

    }

    public void Exchange(EntityComponent opponent)
    {

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
