using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The base class representing a card in runtime.
/// </summary>
public abstract class CardComponent : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// The asset use to set this card datas.
    /// </summary>
    [SerializeField]
    private CardSO _cardSO = null;

    [SerializeField]
    private Image _image = null;

    #endregion

    private void Start()
    {

    }

    #region Public API

    ///<inheritdoc cref="_cardSO"/>
    public CardSO CardSO => _cardSO;

    /// <summary>
    /// Set the card data asset.
    /// </summary>
    /// <param name="cardSO"></param>
    public void SetAsset(CardSO cardSO)
    {
        _cardSO = cardSO;
        
        //Debug.Log("Sprite : " + _cardSO.Renderer);
        _image.sprite = _cardSO.Renderer;
    }

    #endregion
}
