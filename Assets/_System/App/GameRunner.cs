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

    [Header("Dice")]

    [SerializeField]
    private DiceComponent _dice = null;


    private Pile _piles = null;

    private EntityComponent _currentPlayer = null;


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

        foreach (var item in _piles.Piles.Keys)
        {
            Debug.Log("Cards" + item);
        }
    }

    private void PlayerTurn()
    {
        _currentGameState = GameState.PlayerTurn;
        _currentPlayer = _player;

        // Roll Dice
        int dicesValue = 0;
        //for (int i = 0; i < _player.NbDice; i++)
        //{
        //    dicesValue += _dice.Roll(_config.NbDices);
        //}

        //Player Effects
        foreach (CardComponent card in _player.Cards)
        {
            if (card.CardSO.ActivationNumber != dicesValue || (card.CardSO.ActivationType != CardActivationType.SelfTurn && card.CardSO.ActivationType != CardActivationType.AllTurn))
                continue;

            card.ApplyEffect(_player, _ai);
        }

        //Opponent Effect
        foreach (CardComponent card in _ai.Cards)
        {
            if (card.CardSO.ActivationNumber != dicesValue || (card.CardSO.ActivationType != CardActivationType.OpponentTurn && card.CardSO.ActivationType != CardActivationType.AllTurn))
                continue;

            card.ApplyEffect(_ai, _player);
        }

        // Purchase Card/Building
        // foreach cards in pioche > card.Buy() -> will Build a card

        // Check win condition
    }

    public bool BuyCard(CardSO card)
    {
        if ((_currentGameState == GameState.PlayerTurn && _currentPlayer is not PlayerComponent) || (_currentGameState == GameState.AITurn && _currentPlayer is not AIComponent))
            return false;

        if (!_piles.DrawCard(card))
            return false;

        return _currentPlayer.BuyCard(card);
    }

    private void AITurn()
    {
        _currentGameState = GameState.PlayerTurn;
        _currentPlayer = _ai;   

        // Roll Dice
        // Effects
        // Purchase Card/Building
    }


    #endregion
}
