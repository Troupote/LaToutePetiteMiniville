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

    private CardSO _selectedCardToBuy = null;


    #endregion

    #region Lifecycle

    void Start()
    {
        Init();
        PlayerTurn(); // Start with the player's turn as soon as game runner is created.

        _boardCanvas.gameObject.SetActive(false);
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
        Debug.Log($"Processing buy card...");

        bool canBuyAnyCard = false;
        foreach (CardSO card in _piles.Piles.Keys)
        {
            if (card.Cost <= _currentPlayer.Coins)
            {
                canBuyAnyCard = true;
                break;
            }
        }

        if (!canBuyAnyCard)
        {
            Debug.LogWarning("No cards available for the player to purchase.");
            ProcessNextStep();
            yield break;
        }

        Debug.Log("Current player :" + _currentPlayer);

        _boardCanvas.gameObject.SetActive(true);

        if (_currentPlayer is AIComponent)
        {
            // Gestion pour l'IA
            List<CardSO> availableCards = _piles.GetAvailableCards();

            if (availableCards.Count == 0)
            {
                Debug.Log("No cards available for AI to purchase.");
                ProcessNextStep();
                yield break;
            }

            _selectedCardToBuy = availableCards[Random.Range(0, availableCards.Count)];
            Debug.Log($"AI selected card: {_selectedCardToBuy.name}");

            if (TryBuyCard(_currentPlayer, _selectedCardToBuy))
            {
                _currentPlayer.BuyCard(_selectedCardToBuy);
                Debug.Log($"AI successfully purchased card: {_selectedCardToBuy.name}");
            }
            else
            {
                Debug.LogError("AI failed to purchase card.");
            }
        }
        else
        {
            // Gestion pour le joueur
            Debug.Log($"Waiting for a card selection...");
            while (true)
            {
                // Attendre que le joueur sélectionne une carte
                yield return new WaitUntil(() => _selectedCardToBuy != null);

                Debug.Log($"Player selected card: {_selectedCardToBuy.name}");

                // Tenter d'acheter la carte
                if (TryBuyCard(_currentPlayer, _selectedCardToBuy))
                {
                    _currentPlayer.BuyCard(_selectedCardToBuy);
                    Debug.Log($"Player successfully purchased card: {_selectedCardToBuy.name}");
                    break; // Achat réussi, sortir de la boucle
                }
                else
                {
                    Debug.LogWarning("Purchase failed. Waiting for another selection...");
                    _selectedCardToBuy = null; // Réinitialiser pour permettre une nouvelle sélection
                }
            }
        }

        Debug.Log("COins !!!! : "+_currentPlayer.Coins.ToString());

        // Fin de la phase d'achat
        _selectedCardToBuy = null;
        _boardCanvas.gameObject.SetActive(false);
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

    public EntityComponent CurrentPlayer => _currentPlayer;
    public GameConfigSO Config => _config;


    public bool TryBuyCard(EntityComponent player, CardSO card)
    {
        Debug.Log($"Attempting to buy card: {card.Name} for player: {player.name}");

        if (player.Coins < card.Cost)
        {
            Debug.LogError($"Not enough coins to buy the card: {card.Name}");
            return false;
        }

        foreach (CardComponent cardComp in player.Cards)
        {
            if (cardComp == card && cardComp.CardSO is BuildingSO building && building.IsUnique)
                return false;
        }

        _selectedCardToBuy = card;

        Debug.Log($"{player.name} allowed to buy card: {card.Name}");
        return true;

    }
}
