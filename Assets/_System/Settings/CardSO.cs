using UnityEngine;

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

    [Tooltip("The card main color.")]
    [SerializeField]
    private Color _color = Color.black;

    [Tooltip("The amount of coin requiered to buy this card.")]
    [SerializeField]
    private int _cost = 0;

    #endregion

    #region Public API

    ///<inheritdoc cref="_name"/>
    public string Name => _name;

    ///<inheritdoc cref="_description"/>
    public string Description => _description;

    ///<inheritdoc cref="_color"/>
    public Color Color => _color;

    ///<inheritdoc cref="_cost"/>
    public int Cost => _cost;

    #endregion
}
