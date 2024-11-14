using UnityEngine;

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
    public void ApplyEffect(EntityComponent user, EntityComponent opponent)
    {
        switch (_cardSO.Effect)
        {
            case CardEffectSO_GainCoin:
                CardEffectSO_GainCoin e = _cardSO.Effect as CardEffectSO_GainCoin;
                user.IncrementCoins(e.GainCoinAmount);
                break;

            case CardEffectSO_UnlockDice:
                break;

            case CardEffect_Exchange:
                break;
        }

    }

    #endregion
}
