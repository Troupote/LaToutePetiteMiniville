using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The base card asset.
/// </summary>
public abstract class CardSO : ScriptableObject
{
    #region Fields

    [Tooltip("The debug name.")]
    [SerializeField]
    private string _name = "";

    [Tooltip("The card description.")]
    [SerializeField]
    private string _description = "";

    [Tooltip("The card renderer")]
    [SerializeField]
    private Sprite _renderer = null;

    [Tooltip("The card prefab")]
    [SerializeField]
    private GameObject _prefab = null;

    [Tooltip("The card main color.")]
    [SerializeField]
    private CardActivationType _activationType = CardActivationType.AllTurn;

    [Tooltip("The value required when rolling dice to trigger this card.")]
    [SerializeField]
    private int _activationNumber = 0;

    [Tooltip("The amount of coin requiered to buy this card.")]
    [SerializeField]
    private int _cost = 0;

    [Tooltip("The card effect when activated.")]
    [SerializeField]
    private CardEffectSO _effect = null;

    #endregion

    #region Public API

    ///<inheritdoc cref="_name"/>
    public string Name => _name;

    ///<inheritdoc cref="_description"/>
    public string Description => _description;

    ///<inheritdoc cref="_renderer"/>
    public Sprite Renderer => _renderer;

    ///<inheritdoc cref="_color"/>
    public CardActivationType ActivationType => _activationType;

    /// <inheritdoc cref="_activationNumber"/>
    public int ActivationNumber => _activationNumber;

    ///<inheritdoc cref="_cost"/>
    public int Cost => _cost;

    ///<inheritdoc cref="_effect"/>
    public CardEffectSO Effect => _effect;


    /// <summary>
    /// Build a card using this <see cref="CardSO"/>
    /// </summary>
    /// <returns>Returns the built <see cref="CardComponent"/>.</returns>
    public CardComponent Build(Vector3 position,Quaternion rotation, Transform parent )
    {

        GameObject cardObj = Instantiate(_prefab,position,rotation,parent);

        if (!cardObj.TryGetComponent<CardComponent>(out CardComponent card))
        {
            Debug.LogError("Failed building card");
            return null;
        }
        card.SetAsset(this);

        return card;
    }

    /// <summary>
    /// Apply the card effect.
    /// </summary>
    public void ApplyEffect(EntityComponent user, EntityComponent opponent, Action onDone)
    {
        _effect.ApplyEffect(user, opponent, onDone);
    }

    #endregion
}
