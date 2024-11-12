using UnityEngine;

[CreateAssetMenu(fileName = "EntityConfigSO", menuName = "Game/Entity/Config")]
public class EntityConfigSO : ScriptableObject
{
    #region Fields

    [Tooltip("The number of dices the entity has to play with.")]
    [SerializeField]
    private int _nbDices = 0;

    [Tooltip("The number of coins the entity has at the beginning.")]
    [SerializeField]
    private int _coins = 0;

    #endregion

    #region Public API

    ///<inheritdoc cref="_nbDices"/>
    public int NbDices => Mathf.Max(0, _nbDices);

    ///<inheritdoc cref="_coins"/>
    public int Coins => Mathf.Max(0, _coins);

    #endregion

}
