using System.Linq;
using DG.Tweening;
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

    [Header("Music")]

    [SerializeField]
    private AudioSource GameMusic;

    [Tooltip("Doit �tre entre 0 et 1, et d�fini avant de run")]
    [SerializeField]
    [Range(0,1)]
    private float Volume = 0.6f;

    //@todo Pile field 

    private Pile _piles = null;

    private EntityComponent _currentPlayer=null;    

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
        GameMusic.Play();
        GameMusic.DOFade(Volume, 1);
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
       if((_currentGameState == GameState.PlayerTurn && _currentPlayer is not PlayerComponent) || (_currentGameState == GameState.AITurn && _currentPlayer is not AIComponent))
       return false;

       if(! _piles.DrawCard(card))
       return false;

       return _currentPlayer.BuyCard(card);
    }

    //private void AITurn()
    //{
    //    // Roll Dice
    //    // Effects
    //    // Purchase Card/Building
    //}


    #endregion
}
