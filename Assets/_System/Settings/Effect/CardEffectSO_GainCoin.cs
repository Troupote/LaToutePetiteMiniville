using UnityEngine;

[CreateAssetMenu(fileName = "GainCoin", menuName = "Game/Effect/Gain Coin", order = 0)]
public class CardEffectSO_GainCoin : CardEffectSO
{
    #region Fields

    [Tooltip("The amount of coins gained")]
    [SerializeField]
    private int _gainCoin = 0;

    #endregion

    #region Public API

    ///<inheritdoc cref="_gainCoin"/>
    public int GainCoin => _gainCoin;  // équivalent get

    #endregion
}