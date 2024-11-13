using UnityEngine;

public abstract class CardEffectSO : ScriptableObject
{

}


public class CardEffectSO_GainCoin : CardEffectSO
{
    [SerializeField] private int _gainCoin;

    public int GainCoin => _gainCoin;  // équivalent get

    public int GainCoin2 { get => _gainCoin; set => _gainCoin = value; }   // équivalent get set la flèche == return
}

