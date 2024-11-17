using System;
using Unity.VisualScripting;
using UnityEngine;
using static GameRunner;

/// <summary>
/// The base class representing a card in runtime.
/// </summary>
public abstract class CardComponent : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// The asset use to set this card datas.
    /// </summary>
    private CardSO _cardSO = null;

    #endregion

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
    }

    ///Apply the card effect.
    public void ApplyEffect(EntityComponent user, EntityComponent opponent, Action onDone)
    {
        _cardSO.ApplyEffect(user, opponent, onDone);
    }

    #endregion
}
