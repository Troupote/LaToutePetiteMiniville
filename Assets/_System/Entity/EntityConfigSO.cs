using UnityEngine;

[CreateAssetMenu(fileName = "EntityConfigSO", menuName = "Game/Entity/Config")]
public class EntityConfigSO : ScriptableObject
{
    #region Fields

    [Tooltip("The number of coins the entity has at the beginning.")]
    [SerializeField]
    private int _coins = 0;


    [Tooltip("The initial cards.")]
    [SerializeField]
    private CardSO[] _initialCards = { };



    #endregion

    #region Public API

    ///<inheritdoc cref="_coins"/>
    public int Coins => Mathf.Max(0, _coins);

    ///<inheritdoc cref="_initialCards"/>
    public CardSO[] InitialCards => _initialCards;

    #endregion

}
