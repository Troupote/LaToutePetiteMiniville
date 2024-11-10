using UnityEngine;

public class GameRunner : MonoBehaviour
{
    #region Nested

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
