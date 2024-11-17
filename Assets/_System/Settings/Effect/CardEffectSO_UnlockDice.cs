using System;
using UnityEngine;
[CreateAssetMenu(fileName = "CardEffectSO_UnlockDice", menuName = "Game/Effect/Unlock Dice", order = 0)]

public class CardEffectSO_UnlockDice : CardEffectSO
{
    public override void ApplyEffect(EntityComponent user, EntityComponent opp, Action onDone)
    {
        //user.UnlockDice();
        onDone.Invoke();
    }
}