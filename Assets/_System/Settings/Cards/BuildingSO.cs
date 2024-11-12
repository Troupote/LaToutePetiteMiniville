using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "Game/Card/Building", order = 0)]
public class BuildingSO : CardSO
{
    #region Fields

    [Tooltip("Is this building unique ?")]
    [SerializeField]
    private bool _isUnique = false;

    #endregion

    #region Public API

    ///<inheritdoc cref=" _isUnique"/>
    public bool IsUnique => _isUnique;

    #endregion
}
