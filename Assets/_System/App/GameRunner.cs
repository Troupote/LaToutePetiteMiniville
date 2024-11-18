using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    private DiceManager _diceManager = null;

    [Header("Canvas")]
    [SerializeField]
    private Canvas _boardCanvas = null;

    [Header("Sound")]
    [SerializeField]
    private AudioSource _gameMusic;

    [Tooltip("Sound and music volume. Should be between 0 and 1, set before running")]
    [SerializeField]
    [Range(0, 1)]
    private float _volume = 0.6f;

    /// <summary>
    /// The deck/pile used for drawing cards
    /// </summary>
    private Pile _piles = null;

    /// <summary>
    /// The current player.
    /// </summary>
    private EntityComponent _currentPlayer = null;

    /// <summary>
    /// The current opponent.
    /// </summary>
    private EntityComponent _currentOpponent = null;

    /// <summary>
    /// Current game state
    /// </summary>
    private GameState _currentGameState;

    /// <summary>
    /// Tracks the current step in the player's turn
    /// </summary>
    private int _currentProcessIndex = 0;

    /// <summary>
    /// List of steps in the current player's turn
    /// </summary>
    private List<IEnumerator> _processes = new();

    /// <summary>
    /// Value of the dice roll in the current turn.
    /// </summary>
    private int _currentDiceRollValue = 0;

    private CardSO _cardToBuy = null;


    #endregion

    #region Lifecycle

    void Start()
    {
        _boardCanvas.gameObject.SetActive(false);

        Init();
        StartTurn(_player, _ai);

    }

    #endregion

    #region Private API

    /// <summary>
    /// Initializes game resources, like piles and music.
    /// </summary>
    private void Init()
    {
        _piles = new Pile(_config.CardsArchetypes, _config.StackSize);
        _gameMusic.Play();
        _gameMusic.DOFade(_volume, 1); // Fade in game music
    }

    /// <summary>
    /// Begins the turn for a given player
    /// </summary>
    private void StartTurn(EntityComponent currentPlayer, EntityComponent currentOpponent)
    {
        _currentPlayer = currentPlayer;
        _currentOpponent = currentOpponent;
        _currentGameState = currentPlayer is PlayerComponent ? GameState.PlayerTurn : GameState.AITurn;

        Debug.Log($"{currentPlayer.name} Turn");

        // Initialize turn steps
        _processes.Clear();
        _processes.Add(ProcessRollDice());
        _processes.Add(ProcessEffects());
        _processes.Add(ProcessBuyCard());

        _currentProcessIndex = 0;

        if (_processes.Count > 0)
            StartCoroutine(_processes[_currentProcessIndex]);
    }

    /// <summary>
    /// Advances to the next step in the current player process.
    /// </summary>
    private void NextProcess()
    {
        if (_currentProcessIndex + 1 < _processes.Count)
        {
            _currentProcessIndex++;
            StartCoroutine(_processes[_currentProcessIndex]);
        }
        else
        {
            _currentDiceRollValue = 0;
            Debug.Log("End of turn.");
            EndTurn();
        }
    }

    /// <summary>
    /// Handles the dice roll phase.
    /// </summary>
    private IEnumerator ProcessRollDice()
    {
        Debug.Log("Rolling Dice...");

        if (!_diceManager.diceLaunch)
        {
            _diceManager.resultFinal = 0;
            _diceManager.diceLaunch = true;
            _diceManager.CreateDice(_currentPlayer.DiceCount);
        }

        // Wait until the dice roll is complete
        while (_diceManager.diceLaunch)
        {
            if (_diceManager.resultFinal != 0)
            {
                _diceManager.diceLaunch = false;
                _currentDiceRollValue = _diceManager.resultFinal;
                Debug.Log($"Dice result: {_diceManager.resultFinal}");
            }            yield return null;
        }

        Debug.Log("End Roll Dice");
        NextProcess();
    }

    /// <summary>
    /// Handles the effects triggered by the dice roll.
    /// </summary>
    private IEnumerator ProcessEffects()
    {
        Debug.Log("Processing Effects...");
        Debug.LogWarning("Effects for dice roll: " + _currentDiceRollValue);

        Queue<CardEffectSO> playerEffectsQueue = new Queue<CardEffectSO>();

        foreach (CardComponent card in _currentPlayer.Cards)
        {
            if ((card.CardSO.ActivationType == CardActivationType.SelfTurn || card.CardSO.ActivationType == CardActivationType.AllTurn)
                && card.CardSO.ActivationNumber == _currentDiceRollValue)
            {
                playerEffectsQueue.Enqueue(card.CardSO.Effect);
                Debug.Log($"Enqueued effect of card {card.CardSO.Name} for current player.");
            }
        }

        Debug.LogWarning("player effcts to process count : " + playerEffectsQueue.Count);

        foreach (var card in playerEffectsQueue)
            Debug.LogWarning("Player effects activated: " + card);


        ApplyNextEffect(playerEffectsQueue, _currentPlayer, _currentOpponent);


        yield return new WaitUntil(() => playerEffectsQueue.Count == 0);

        Queue<CardEffectSO> opponentEffectsQueue = new Queue<CardEffectSO>();

        foreach (CardComponent card in _currentOpponent.Cards)
        {
            if ((card.CardSO.ActivationType == CardActivationType.OpponentTurn || card.CardSO.ActivationType == CardActivationType.AllTurn)
                && card.CardSO.ActivationNumber == _currentDiceRollValue)
            {
                opponentEffectsQueue.Enqueue(card.CardSO.Effect);
                Debug.Log($"Enqueued effect of card {card.CardSO.Name} for opponent.");
            }
        }

        Debug.LogWarning("opponent effcts to process count : " + opponentEffectsQueue.Count);


        ApplyNextEffect(opponentEffectsQueue, _currentOpponent, _currentPlayer);

        yield return new WaitUntil(() => opponentEffectsQueue.Count == 0);

        Debug.Log("End Process Effects");

        Debug.Log("Next Step");
        NextProcess();
        yield return null;
    }

    /// <summary>
    /// Handles the card-buying process.
    /// </summary>
    private IEnumerator ProcessBuyCard()
    {
        Debug.Log($"Processing buy card...");

        _boardCanvas.gameObject.SetActive(true);

        bool canBuy = CanBuy();

        if (!canBuy)
        {
            Debug.LogWarning("No more cards available.");
            NextProcess();
            yield break;
        }

        while (_cardToBuy == null)
            yield return null;

        yield return _currentPlayer.ProcessBuyCard(_piles, _cardToBuy);

        _cardToBuy = null;
        _boardCanvas.gameObject.SetActive(false);


        NextProcess();
    }


    private bool CanBuy()
    {
        List<CardSO> availableCards = _piles.GetAvailableCards();

        if (availableCards.Count <= 0)
            return false;

        foreach (CardSO card in availableCards)
        {
            if (card.Cost <= _currentPlayer.Coins)
                return true;
        }

        return false;
    }



    /// <summary>
    /// Ends the current turn and transitions to the next.
    /// </summary>
    private void EndTurn()
    {
        if (_currentPlayer is PlayerComponent)
        {
            Debug.Log("Player Turn Ended");
            StartTurn(_ai, _player);  // Switch to AI's turn
        }
        else
        {
            Debug.Log("AI Turn Ended");
            StartTurn(_player, _ai);  // Switch back to Player's turn
        }
    }

    /// <summary>
    /// Applies the next effect in the queue, recursively processing all effects.
    /// </summary>
    private void ApplyNextEffect(Queue<CardEffectSO> effectsQueue, EntityComponent user, EntityComponent opponent)
    {
        if (effectsQueue.Count <= 0)
            return;

        CardEffectSO effect = effectsQueue.Dequeue();
        effect.ApplyEffect(user, opponent, () => ApplyNextEffect(effectsQueue, user, opponent));
    }

    #endregion

    public EntityComponent CurrentPlayer => _currentPlayer;
    public GameConfigSO Config => _config;


    public void SelectCardToBuy(EntityComponent player, CardSO card)
    {
        Debug.Log($"Attempting to buy card: {card.Name} for player: {player.name}");
        _cardToBuy = card;
    }
}
