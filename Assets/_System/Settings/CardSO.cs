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
    private Image _renderer = null;

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

    #endregion

    #region Public API

    ///<inheritdoc cref="_name"/>
    public string Name => _name;

    ///<inheritdoc cref="_description"/>
    public string Description => _description;

    ///<inheritdoc cref="_renderer"/>
    public Image Renderer => _renderer;

    ///<inheritdoc cref="_color"/>
    public CardActivationType ActivationType => _activationType;

    /// <inheritdoc cref="_activationNumber"/>
    public int ActivationNumber => _activationNumber;

    ///<inheritdoc cref="_cost"/>
    public int Cost => _cost;

    public GameObject Build()
    {
       GameObject card = Instantiate(_prefab);

        if (!card.TryGetComponent<CardComponent>(out CardComponent cardComponent))
            Debug.LogError("Failed building card");

        cardComponent.SetAsset(this);

        return card;
    }
    public void ApplyEffect()
    {
    }

    #endregion
}
