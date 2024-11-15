using DG.Tweening;
using System;
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
    private DiceComponent _dice = null;

    [Header("Music")]

    [SerializeField]
    private AudioSource GameMusic;

    [Tooltip("Doit �tre entre 0 et 1, et d�fini avant de run")]
    [SerializeField]
    [Range(0, 1)]
    private float Volume = 0.6f;

    //@todo Pile field 

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
        GameMusic.Play();
        GameMusic.DOFade(Volume, 1);
    }

    private void PlayerTurn()
    {
        _currentGameState = GameState.PlayerTurn;
        _currentPlayer = _player;

        // Dice 
        int dicesValue = 0;
        // Dice.Roll(_currentPlayer);

        Queue<CardEffectSO> effectsQueue = new Queue<CardEffectSO>();


        // Process player effects
        foreach (CardComponent card in _player.Cards)
        {
            if (card.CardSO.ActivationNumber != dicesValue ||
                (card.CardSO.ActivationType != CardActivationType.SelfTurn && card.CardSO.ActivationType != CardActivationType.AllTurn))
                continue;

            effectsQueue.Enqueue(card.CardSO.Effect);
        }

        //Entry point to apply effect
        //ApplyNextEffect(_currentPlayer, _ai, ApplyNextEffect);

        // Process opponent effects
        foreach (CardComponent card in _ai.Cards)
        {
            if (card.CardSO.ActivationNumber != dicesValue ||
                (card.CardSO.ActivationType != CardActivationType.OpponentTurn && card.CardSO.ActivationType != CardActivationType.AllTurn))
                continue;

            card.ApplyEffect(_ai, _player);
        }
    }

    private void AITurn()
    {
        _currentGameState = GameState.AITurn;
        _currentPlayer = _ai;

        // Roll Dice
        // Effects
        // Purchase Card/Building
    }

    private void ApplyNextEffect(Queue<CardEffectSO> effectsQueue, EntityComponent user, EntityComponent opponent, Action onDone)
    {
        if (effectsQueue.Count <= 0)
        {
            //Check if current is player
            EndPlayerTurn();

            onDone.Invoke();
            return;
        }

        CardEffectSO effect = effectsQueue.Dequeue();
        effect.ApplyEffect(user, opponent);
        //ApplyNextEffect(effectsQueue, user, opponent, ApplyNextEffect);
    }

    /// <summary>
    /// End effect callabck called to process the next effect.
    /// </summary>
    private void OnEffectDone()
    {
        EndPlayerTurn();
    }

    private void EndPlayerTurn()
    {
        Debug.Log("End player turn");
        _currentGameState = GameState.AITurn;
        AITurn();
    }

    #endregion

    #region Public API

    public bool BuyCard(CardSO card)
    {
        if ((_currentGameState == GameState.PlayerTurn && _currentPlayer is not PlayerComponent) || (_currentGameState == GameState.AITurn && _currentPlayer is not AIComponent))
            return false;

        if (!_piles.DrawCard(card))
            return false;

        return _currentPlayer.BuyCard(card);
    }

    #endregion

}
