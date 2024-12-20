using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Exchange", menuName = "Game/Effect/Exchange")]
public class CardEffect_Exchange : CardEffectSO
{
    public override void ApplyEffect(EntityComponent user, EntityComponent opp, Action onDone)
    {
        user.Exchange(opp);
        onDone.Invoke();
    }
}
