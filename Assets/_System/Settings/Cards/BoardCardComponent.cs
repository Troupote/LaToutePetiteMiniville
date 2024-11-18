using UnityEngine;
using UnityEngine.UI;

public class BoardCardComponent : MonoBehaviour
{
    [SerializeField]
    private Button _cardButton = null;

    [SerializeField]
    private Image _renderer = null;

    private GameRunner _gameRunner = null;
    private CardSO _card = null;
    private Transform _targetHand = null;
    private EntityComponent _currentPlayer = null;

    private void Start()
    {
        _gameRunner = FindFirstObjectByType<GameRunner>();

        if (_card == null)
            Debug.Log("CardSO missing", this);

        _cardButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (_cardButton != null)
            _cardButton.onClick.AddListener(HandleCardSelected);
    }

    private void OnDisable()
    {
        if (_cardButton != null)
            _cardButton.onClick.RemoveListener(HandleCardSelected);

        if (_currentPlayer != null)
            _currentPlayer.OnBuyCard.RemoveListener(HandleBuyCard);
    }

    public void SetBoardCard(BuildingSO card)
    {
        _card = card;
        _renderer.sprite = card.Renderer;
        name = _card.name;
    }

    public void HandleCardSelected()
    {
        if (_gameRunner == null)
        {
            Debug.LogError("GameRunner not found in scene.");
            return;
        }

        _currentPlayer = _gameRunner.CurrentPlayer;
        _currentPlayer.OnBuyCard.AddListener(HandleBuyCard);
        _gameRunner.SelectCardToBuy(_currentPlayer, _card);
    }

    private void HandleBuyCard(EntityComponent player, CardSO card)
    {
        Debug.Log("BUY !");

        _targetHand = _currentPlayer is PlayerComponent ? FindFirstObjectByType<PlayerHand>().transform : FindFirstObjectByType<AIHand>().transform;
        Debug.LogWarning("Container :." + _targetHand.name);

        CardComponent newCard = card.Build(transform.position, Quaternion.identity, _targetHand);

        _currentPlayer.Cards.Add(newCard);

        ResizeCardContainerComponent container = _targetHand.GetComponent<ResizeCardContainerComponent>();
        if (container != null)
        {
            container.Resize();
        }
    }
}
