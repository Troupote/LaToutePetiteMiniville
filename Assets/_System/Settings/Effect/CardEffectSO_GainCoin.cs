using UnityEngine;


[CreateAssetMenu(fileName = "GainCoin", menuName = "Game/Effect/Gain Coin", order = 0)]
public class CardEffectSO_GainCoin : CardEffectSO
{
    [SerializeField] private int _gainCoin;
    public int GainCoin => _gainCoin;  // équivalent get
}