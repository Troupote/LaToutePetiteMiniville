using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GainCoin", menuName = "Game/Effect/Gain Coin", order = 0)]
public class CardEffectSO_GainCoin : CardEffectSO
{
    #region Fields

    [Tooltip("The amount of coins gained")]
    [SerializeField]
    private int _gainCoinAmount = 0;

    #endregion

    #region Public API

    ///<inheritdoc cref="_gainCoinAmount"/>
    public int GainCoinAmount => _gainCoinAmount; 

    public override void ApplyEffect(EntityComponent user, EntityComponent opp, Action onDone)
    {
        user.Add_SubstarctCoins(GainCoinAmount);
        onDone.Invoke();
    }

    #endregion
}