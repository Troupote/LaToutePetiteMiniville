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
    private DiceManagerScript _diceScript = null;

    [Header("Music")]

    [SerializeField]
    private AudioSource _gameMusic;

    [Tooltip("Doit etre entre 0 et 1, et defini avant de run")]
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

    private void Update()
    {

        PlayerTurn();
    }
    #endregion

    #region Private API

    private void Init()
    {
        //Init piles
        _piles = new Pile(_config.CardsArchetypes, _config.StackSize);
        _gameMusic.Play();
        _gameMusic.DOFade(Volume, 1);
    }

    private void PlayerTurn()
    {
        _currentGameState = GameState.PlayerTurn;
        _currentPlayer = _player;

        // Dice 
        int dicesValue = 0;
        if (!_diceScript.diceLaunch) 
        {
            _diceScript.resultFinal = 0;
            _diceScript.create_dice(2);
            _camera.transform.position = new Vector3((float)(60.61),(float)(5), (float)(-31.6));
            _camera.transform.rotation = new Quaternion(Mathf.Deg2Rad*58,0,0,1);
        }
        if (_diceScript.resultFinal != 0 && _diceScript.diceLaunch)
        {
            _diceScript.diceLaunch = false;
            dicesValue = _diceScript.resultFinal; 
            _camera.transform.position = new Vector3((float)(0), (float)(0), (float)(0));
            _camera.transform.rotation = new Quaternion(0, 0, 0, 1);
            Debug.Log(_diceScript.resultFinal);
        }

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

    public bool BuyCard(CardSO card)
    { 
        
        if((_currentGameState == GameState.PlayerTurn && _currentPlayer is not PlayerComponent) || (_currentGameState == GameState.AITurn && _currentPlayer is not AIComponent))
        return false;

        if(! _piles.DrawCard(card))
        return false;

        return _currentPlayer.BuyCard(card);
    }

    private void EndPlayerTurn()
    {
        Debug.Log("End player turn");
        _currentGameState = GameState.AITurn;
        AITurn();
    }

    #endregion

}
