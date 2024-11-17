using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    private DiceManager _diceManager = null;

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

    #endregion

    #region Lifecycle

    void Start()
    {
        Init();
        PlayerTurn(); // Start with the player's turn as soon as game runner is created.
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
    /// Begins the player's turn.
    /// </summary>
    private void PlayerTurn()
    {
        _currentPlayer = _player;
        _currentOpponent = _ai;
        _currentGameState = GameState.PlayerTurn;

        Debug.Log("Enter Player Turn");

        // Initialize turn steps
        _processes.Clear();
        _processes.Add(ProcessRollDice());
        _processes.Add(ProcessEffects());
        _processes.Add(ProcessBuyCard());

        _currentProcessIndex = 0;

        // Start the first step automatically
        if (_processes.Count > 0)
            StartCoroutine(_processes[_currentProcessIndex]);
    }

    /// <summary>
    /// Begins the AI's turn.
    /// </summary>
    private void AITurn()
    {
        _currentPlayer = _ai;
        _currentOpponent = _player;
        _currentGameState = GameState.AITurn;

        Debug.Log("Enter AI Turn");

        // Initialize turn steps
        _processes.Clear();
        _processes.Add(ProcessRollDice());
        _processes.Add(ProcessEffects());
        _processes.Add(ProcessBuyCard());

        _currentProcessIndex = 0;

        // Start the first step automatically
        if (_processes.Count > 0)
            StartCoroutine(_processes[_currentProcessIndex]);
    }

    /// <summary>
    /// Advances to the next step in the player's or AI's turn.
    /// </summary>
    private void ProcessNextStep()
    {
        if (_currentProcessIndex + 1 < _processes.Count)
        {
            _currentProcessIndex++;
            StartCoroutine(_processes[_currentProcessIndex]);
        }
        else
        {
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
            }

            yield return null;
        }

        Debug.Log("End Roll Dice");
        ProcessNextStep();
    }

    /// <summary>
    /// Handles the effects triggered by the dice roll.
    /// </summary>
    private IEnumerator ProcessEffects()
    {
        Debug.Log("Processing Effects...");

        Queue<CardEffectSO> effectsQueue = new Queue<CardEffectSO>();

        // Process current player's card effects
        foreach (CardComponent card in _currentPlayer.Cards)
        {
            if (card.CardSO.ActivationType == CardActivationType.SelfTurn || card.CardSO.ActivationType == CardActivationType.AllTurn)
            {
                effectsQueue.Enqueue(card.CardSO.Effect);
            }
        }

        // Process opponent's card effects
        foreach (CardComponent card in _currentOpponent.Cards)
        {
            if (card.CardSO.ActivationType == CardActivationType.OpponentTurn || card.CardSO.ActivationType == CardActivationType.AllTurn)
            {
                effectsQueue.Enqueue(card.CardSO.Effect);
            }
        }

        ApplyNextEffect(effectsQueue, _currentPlayer, _currentOpponent);

        Debug.Log("End Process Effects");
        yield return null;
    }

    /// <summary>
    /// Handles the card-buying process.
    /// </summary>
    private IEnumerator ProcessBuyCard()
    {
        Debug.Log($"{_currentPlayer.GetType().Name} is selecting a card to buy...");

        CardSO selectedCard = null;

        if (_currentPlayer is AIComponent)
        {
            List<CardSO> availableCards = _piles.GetAvailableCards();

            if (availableCards.Count == 0)
            {
                Debug.Log("No cards available for AI to purchase.");
                ProcessNextStep();
                yield break;
            }

            selectedCard = availableCards[Random.Range(0, availableCards.Count)];
            Debug.Log($"AI selected card: {selectedCard.name}");
        }
        else
        {
            while (selectedCard == null)
                yield return null;

            Debug.Log($"Player selected card: {selectedCard.name}");
        }

        if (_piles.DrawCard(selectedCard) && _currentPlayer.BuyCard(selectedCard))
        {
            Debug.Log($"{_currentPlayer.GetType().Name} successfully purchased card: {selectedCard.name}");
        }
        else
        {
            Debug.LogWarning($"{_currentPlayer.GetType().Name} failed to purchase the card.");
        }

        ProcessNextStep();
    }



    /// <summary>
    /// Ends the current turn and transitions to the next.
    /// </summary>
    private void EndTurn()
    {
        if (_currentPlayer is PlayerComponent)
        {
            Debug.Log("Player Turn Ended");
            AITurn();
        }
        else
        {
            Debug.Log("AI Turn Ended");
            PlayerTurn();
        }
    }

    /// <summary>
    /// Applies the next effect in the queue, recursively processing all effects.
    /// </summary>
    private void ApplyNextEffect(Queue<CardEffectSO> effectsQueue, EntityComponent user, EntityComponent opponent)
    {
        if (effectsQueue.Count <= 0)
        {
            ProcessNextStep();
            return;
        }

        CardEffectSO effect = effectsQueue.Dequeue();
        effect.ApplyEffect(user, opponent, () => ApplyNextEffect(effectsQueue, user, opponent));
    }

    #endregion
}
