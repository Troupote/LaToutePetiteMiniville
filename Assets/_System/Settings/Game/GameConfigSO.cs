using UnityEngine;

[CreateAssetMenu(fileName = "GameConfigSO", menuName = "Game/Game/Config")]
public class GameConfigSO : ScriptableObject
{
    #region Fields

    [Tooltip("The number of dices the entity has to play with.")]
    [SerializeField]
    private int _nbDices = 0;

    #endregion

    #region Public API

    ///<inheritdoc cref="_nbDices"/>
    public int NbDices => Mathf.Max(0, _nbDices);

    #endregion
}
