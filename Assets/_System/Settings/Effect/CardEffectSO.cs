using System;
using UnityEngine;

public abstract class CardEffectSO : ScriptableObject
{
    public abstract void ApplyEffect(EntityComponent user, EntityComponent opp, Action onDone);
}




