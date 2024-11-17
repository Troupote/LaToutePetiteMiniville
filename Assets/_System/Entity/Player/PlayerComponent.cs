using UnityEngine;

/// <summary>
/// Represents a player in runtime.
/// </summary>
public class PlayerComponent : EntityComponent
{
    [SerializeField]
    CardSO cardTest = null;

    private void Start()
    {
        base.Init();

        BuyCard(cardTest);

        var c = cardTest.Build();

        Cards.Add(c);
    }
}
