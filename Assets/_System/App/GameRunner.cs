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


        Debug.Log("Piles in pile" + _piles.Piles);

        Debug.Log("Player cards : " + _player.Cards);
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
    private void AITurn() => StartCoroutine(ProcessAITurn());

    /// <summary>
    /// Advances to the next step in the player's turn.
    /// </summary>
    public void ProcessNextStep()
    {
        if ((_currentPlayer is PlayerComponent && _currentGameState != GameState.PlayerTurn) || (_currentPlayer is AIComponent && _currentGameState != GameState.AITurn))
        {
            Debug.LogWarning("WrOng turn turn !");
            StopAllCoroutines();
            return;
        }

        if (_currentProcessIndex + 1 < _processes.Count)
        {
            _currentProcessIndex++;
            StartCoroutine(_processes[_currentProcessIndex]);
        }
        else
        {
            Debug.Log("End of player turn.");
            EndTurn();
        }
    }

    /// <summary>
    /// Executes the AI's turn in one continuous flow.
    /// </summary>
    private IEnumerator ProcessAITurn()
    {
        _currentPlayer = _ai;
        _currentOpponent = _player;
        _currentGameState = GameState.AITurn;

        Debug.Log("Enter AI Turn");

        // Perform all steps automatically
        yield return ProcessRollDice();
        yield return ProcessEffects();
        yield return ProcessBuyCard();

        EndTurn();
    }

    /// <summary>
    /// Handles the dice roll phase.
    /// </summary>
    private IEnumerator ProcessRollDice()
    {
        Debug.Log("Rolling Dice...");
        //yield return new WaitForSeconds(1); // Simulate dice rolling delay

        if (!_diceManager.diceLaunch)
        {
            _diceManager.resultFinal = 0;
            _diceManager.CreateDice(2); // Create dice for rolling
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
        //yield return new WaitForSeconds(1); // Simulate effects processing delay

        Queue<CardEffectSO> effectsQueue = new Queue<CardEffectSO>();

        // Process current player's card effects
        foreach (CardComponent card in _currentPlayer.Cards)
        {
            if (/*card.CardSO.ActivationNumber == _currentDiceRollValue &&*/
                (card.CardSO.ActivationType == CardActivationType.SelfTurn || card.CardSO.ActivationType == CardActivationType.AllTurn))
            {
                effectsQueue.Enqueue(card.CardSO.Effect);
            }
        }

        // Process opponent's card effects
        foreach (CardComponent card in _currentOpponent.Cards)
        {
            if (card.CardSO.ActivationNumber == _currentDiceRollValue &&
                (card.CardSO.ActivationType == CardActivationType.OpponentTurn || card.CardSO.ActivationType == CardActivationType.AllTurn))
            {
                effectsQueue.Enqueue(card.CardSO.Effect);
            }
        }

        Debug.Log(" Effects queue : " + effectsQueue);


        // Apply the effects sequentially
        ApplyNextEffect(effectsQueue, _currentPlayer, _currentOpponent);

        Debug.Log("End Process Effects");

        yield return null;
    }

    /// <summary>
    /// Handles the card-buying phase.
    /// </summary>
    private IEnumerator ProcessBuyCard()
    {
        Debug.Log("Enter buy card process...");
        yield return new WaitForSeconds(1); // Optionnel : délai avant de commencer la sélection

        CardSO selectedCard = null;

        // Add listener
        //CardComponent.OnCardSelected += (CardSO card) =>
        //{
        //    selectedCard = card;
        //};

        // Wait for a card

        Debug.Log($"Wait for card ...");
        while (selectedCard == null)
            yield return null;

        // Remove listener
        //CardComponent.OnCardSelected -= (CardSO card) =>
        //{
        //    selectedCard = card;
        //};

        Debug.Log($"Selected card : {selectedCard.name}");

        // Buy a card
        if (_piles.DrawCard(selectedCard) && _currentPlayer.BuyCard(selectedCard))
            Debug.Log($"Card : {selectedCard.name}");
        else
            Debug.Log("End Process Buy card");

        Debug.Log("End process");
    }


    /// <summary>
    /// Ends the current player's or AI's turn.
    /// </summary>
    private void EndTurn()
    {
        if (_currentPlayer is PlayerComponent)
            EndPlayerTurn();
        else
            EndAITurn();
    }

    /// <summary>
    /// Ends the player's turn and starts the AI's turn.
    /// </summary>
    private void EndPlayerTurn()
    {
        Debug.Log("Player Turn Ended");
        _currentGameState = GameState.AITurn;
        AITurn();
    }

    /// <summary>
    /// Ends the AI's turn and starts the player's turn.
    /// </summary>
    private void EndAITurn()
    {
        Debug.Log("AI Turn Ended");
        _currentGameState = GameState.PlayerTurn;
        PlayerTurn();
    }

    /// <summary>
    /// Applies the next effect in the queue, recursively processing all effects.
    /// </summary>
    private void ApplyNextEffect(Queue<CardEffectSO> effectsQueue, EntityComponent user, EntityComponent opponent)
    {
        Debug.Log(" Effects queue : " + effectsQueue);

        if (effectsQueue.Count <= 0)
            ProcessNextStep();

        CardEffectSO effect = effectsQueue.Count > 0 ? effectsQueue.Dequeue() : null;

        if (effect == null)
            return;

        Debug.Log("Effect processing : " + effect.name);

        effect.ApplyEffect(user, opponent, () => ApplyNextEffect(effectsQueue, user, opponent));
    }

    #endregion

    #region Public API

    /// <summary>
    /// Attempts to buy a card. Placeholder for future implementation.
    /// </summary>
    public CardSO TryBuyCard(CardSO card)
    {
        return card;
    }

    #endregion
}
