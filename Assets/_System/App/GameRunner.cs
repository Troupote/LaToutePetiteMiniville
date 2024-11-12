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
        PlayerTurnStart,
        RollDice,
        ResolveDiceRoll,
        ActivateEstablishments,
        PurchaseEstablishment,
        EndTurn,
        CheckWinCondition,
        GameOver
    }

    #endregion

    #region

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
