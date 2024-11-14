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

    #region Fields

    [Header("Configuration")]

    [SerializeField]
    private GameConfigSO _config = null;

    [Header("Players")]

    [SerializeField]
    private PlayerComponent _player = null;

    [SerializeField]
    private AIComponent _ai = null;

    //@todo Pile field 

    private Pile _piles = null;

    //@todo Dice

    /// <summary>
    /// The current game state.
    /// </summary>
    private GameState _currentGameState;

    #endregion

    #region Lifecycle

    void Start()
    {
        Init();
    }

    #endregion

    #region Private API

    private void Init()
    {
        //Init piles
        _piles = new Pile(_config.CardsArchetypes, _config.StackSize);
    }

    private void PlayerTurn()
    {
        // Roll Dice
        int dicesValue = 0;
        for (int i = 0; i < _config.NbDices; i++)
        {
            //dicesValue += _dice.Roll(_config.NbDices);
        }

        //Player Effects
        foreach (CardComponent card in _player.Cards)
        {
            if (card.CardSO.ActivationNumber != dicesValue || (card.CardSO.ActivationType != CardActivationType.SelfTurn && card.CardSO.ActivationType != CardActivationType.AllTurn))
                continue;

            card.ApplyEffect(_player);
        }

        //Opponent Effect
        foreach (CardComponent card in _ai.Cards)
        {
            if (card.CardSO.ActivationNumber != dicesValue || (card.CardSO.ActivationType != CardActivationType.OpponentTurn && card.CardSO.ActivationType != CardActivationType.AllTurn))
                continue;

            card.ApplyEffect(_ai);
        }

        // Purchase Card/Building
        // foreach cards in pioche > card.Buy() -> will Build a card

        // Check win condition
    }

    //private void AITurn()
    //{
    //    // Roll Dice
    //    // Effects
    //    // Purchase Card/Building
    //}


    #endregion
}
