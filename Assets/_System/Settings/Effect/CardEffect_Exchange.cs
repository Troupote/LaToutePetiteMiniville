using UnityEngine;


[CreateAssetMenu(fileName = "Exchange", menuName = "Game/Effect/Exchange")]

public class CardEffect_Exchange : CardEffectSO
{
    public override void ApplyEffect(EntityComponent user, EntityComponent opp)
    {
        Debug.Log("Exchange");

        //user.Exchange();
    }
}
