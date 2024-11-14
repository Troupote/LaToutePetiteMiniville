using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameConfigSO", menuName = "Game/Game/Config")]
public class GameConfigSO : ScriptableObject
{
    #region Fields

    [Header("Settings - Dice")]

    [Tooltip("The number of dices the entity has to play with.")]
    [SerializeField]
    private int _nbDices = 0;

    [Header("Settings - Pile")]

    [Tooltip("The number of copies for a card archetype.")]
    [SerializeField]
    private int _stackSize = 0;

    [Tooltip("The list of card archetypes used to initialize the pile.")]
    [SerializeField]
    private CardSO[] _cardsArchetypes = null;

    #endregion

    #region Public API

    ///<inheritdoc cref="_nbDices"/>
    public int NbDices => Mathf.Max(0, _nbDices);

    ///<inheritdoc cref="_cardsArchetypes"/>
    public CardSO[] CardsArchetypes => _cardsArchetypes;

    ///<inheritdoc cref="_nbDices"/>
    public int StackSize => Mathf.Max(0, _stackSize);

    #endregion
}
