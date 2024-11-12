using UnityEngine;

public class GameRunner : MonoBehaviour
{
    #region Nested

    /// <summary>
    /// Represents the different game states.
    /// </summary>
    public enum GameState
    {
        StartGame,
        PlayerTurn,
        AITurn,
        GameOver
    }

    #endregion

    #region

    [SerializeField]
    private PlayerComponent _player = null;

    [SerializeField]
    private AIComponent _aiComponent = null;

    /// <summary>
    /// The current game state.
    /// </summary>
    private GameState _currentMiniVilleGameState;

    #endregion

    void Start()
    {

    }

    void Update()
    {

    }
}
