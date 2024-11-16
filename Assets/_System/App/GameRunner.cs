using DG.Tweening;
using System;
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


    [Header("Camera")]

    [SerializeField]
    private Camera _camera = null;


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

    [Tooltip("Doit etre entre 0 et 1, et defini avant de run")]
    [SerializeField]
    [Range(0, 1)]
    private float _volume = 0.6f;


    /// <summary>
    /// 
    /// </summary>
    private Pile _piles = null;

    /// <summary>
    /// 
    /// </summary>
    private EntityComponent _currentPlayer = null;

    /// <summary>
    /// 
    /// </summary>
    private EntityComponent _currentOpponent = null;

    /// <summary>
    /// The current game state.
    /// </summary>
    private GameState _currentGameState;

    private Coroutine _processRollDiceCoroutine = null;
    private Coroutine _processEffectsCoroutine = null;
    private Coroutine _processBuyCardCoroutine = null;

    /// <summary>
    /// The amount of a roll for the current turn.
    /// </summary>
    private int _currentDiceRollValue = 0;

    #endregion

    #region Lifecycle

    void Start()
    {
        Init();

        PlayerTurn();
    }

    #endregion

    #region Private API

    private void Init()
    {
        //Init piles
        _piles = new Pile(_config.CardsArchetypes, _config.StackSize);
        _gameMusic.Play();
        _gameMusic.DOFade(_volume, 1);
    }

    private void PlayerTurn() => StartCoroutine(ProcessPlayerTurn());

    private void AITurn() => StartCoroutine(ProcessAITurn());

    private IEnumerator ProcessPlayerTurn()
    {
        _currentPlayer = _player;
        _currentOpponent = _ai;

        //yield return ProcessRollDice();

        Debug.Log("Enter Player turn");

        Queue<IEnumerator> processes = new();

        processes.Enqueue(ProcessRollDice());
        processes.Enqueue(ProcessEffects());
        processes.Enqueue(ProcessBuyCard());
        Debug.Log("list created");



        while (processes.Count > 0)
        {
            IEnumerator p = processes.Dequeue();

            if (p == null)
                continue;

            Debug.Log("Proc");
            yield return p;
        }

        //foreach (IEnumerator process in processes)
        //    yield return process;

        Debug.Log("End Player turn");

        yield return null;
    }

    private IEnumerator ProcessAITurn()
    {
        _currentPlayer = _ai;
        _currentOpponent = _player;


        // Roll Dice
        // Effects
        // Purchase Card/Building

        yield return null;

    }

    private IEnumerator ProcessRollDice()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Enter dice process");


        while (_processRollDiceCoroutine != null)
            yield return null;

        if (!_diceManager.diceLaunch)
        {
            _diceManager.resultFinal = 0;
            _diceManager.CreateDice(2);
            _camera.transform.position = new Vector3((float)(60.61), (float)(5), (float)(-31.6));
            _camera.transform.rotation = new Quaternion(Mathf.Deg2Rad * 58, 0, 0, 1);
        }

        while (_diceManager.diceLaunch)
        {
            if (_diceManager.resultFinal != 0)
            {
                _diceManager.diceLaunch = false;
                _currentDiceRollValue = _diceManager.resultFinal;
                _camera.transform.position = new Vector3((float)(0), (float)(0), (float)(0));
                _camera.transform.rotation = new Quaternion(0, 0, 0, 1);

                Debug.Log(_diceManager.resultFinal);
            }

            yield return null;
        }

        _processRollDiceCoroutine = null;

        Debug.Log("End Roll dice");

        yield return null;
    }

    private IEnumerator ProcessEffects()
    {
        yield return new WaitForSeconds(1);

        Debug.Log("Enter Effects process");


        while (_processEffectsCoroutine != null || _processRollDiceCoroutine != null)
            yield return null;

        Queue<CardEffectSO> effectsQueue = new Queue<CardEffectSO>();

        // Process current player effects
        {
            foreach (CardComponent card in _currentPlayer.Cards)
            {
                if (card.CardSO.ActivationNumber != _currentDiceRollValue ||
                    (card.CardSO.ActivationType != CardActivationType.SelfTurn && card.CardSO.ActivationType != CardActivationType.AllTurn))
                    continue;

                effectsQueue.Enqueue(card.CardSO.Effect);
            }

            ApplyNextEffect(effectsQueue, _player, _ai);
        }


        // Process opponent effects
        {
            foreach (CardComponent card in _currentOpponent.Cards)
            {
                if (card.CardSO.ActivationNumber != _currentDiceRollValue ||
                    (card.CardSO.ActivationType != CardActivationType.OpponentTurn && card.CardSO.ActivationType != CardActivationType.AllTurn))
                    continue;

                effectsQueue.Enqueue(card.CardSO.Effect);
            }

            ApplyNextEffect(effectsQueue, _ai, _player);
        }

        effectsQueue.Clear();

        Debug.Log("End process effects");
        _processEffectsCoroutine = null;

        yield return null;
    }

    private IEnumerator ProcessBuyCard()
    {
        yield return new WaitForSeconds(1);

        Debug.Log("Enter buy process");


        while (_processRollDiceCoroutine != null || _processBuyCardCoroutine != null)
            yield return null;

        //Player case
        if ((_currentGameState == GameState.PlayerTurn && _currentPlayer is not PlayerComponent) || (_currentGameState == GameState.AITurn && _currentPlayer is not AIComponent))
            yield break;

        //IA case


        //ask for a card
        //Display card menu  
        CardSO card = null;/*TryBuyCard()*/

        //if (!_piles.DrawCard(card))
        //    yield break;

        //if (!_currentPlayer.BuyCard(card))
        //    yield break;


        EndTurn();

        Debug.Log("End Roll dice");
        _processBuyCardCoroutine = null;

        yield return null;
    }

    private void EndTurn()
    {
        if (_currentPlayer is PlayerComponent)
            EndPlayerTurn();
        else
            EndAITurn();
    }

    private void EndPlayerTurn()
    {
        Debug.Log("End player turn");
        _currentGameState = GameState.AITurn;
        AITurn();
    }

    private void EndAITurn()
    {
        Debug.Log("End ai turn");
        _currentGameState = GameState.PlayerTurn;
        PlayerTurn();
    }

    /// <summary>
    /// Get the next effect handler.
    /// </summary>
    /// <param name="handlers">Returns the next handler if exists.</param>
    private void ApplyNextEffect(Queue<CardEffectSO> effectsQueue, EntityComponent user, EntityComponent opponent)
    {
        CardEffectSO effect = effectsQueue.Count > 0 ? effectsQueue.Dequeue() : null;

        if (effect == null)
            return;

        effect.ApplyEffect(user, opponent, () => ApplyNextEffect(effectsQueue, user, opponent));
    }

    #endregion

    #region Public API

    public CardSO TryBuyCard(CardSO card)
    {
        return card;
    }

    #endregion

}
