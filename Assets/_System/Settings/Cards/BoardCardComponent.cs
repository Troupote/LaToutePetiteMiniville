using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BoardCardComponent : MonoBehaviour
{
    private Button _cardButton = null;

    [SerializeField]
    private Image _renderer = null;

    private GameRunner _gameRunner = null;
    private CardSO _card = null;
    private Transform _targetHand = null;

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
    }

    private void OnMouseOver()
    {
        Debug.Log("Hover");
        transform.DOScale(2, 0.5f);
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
        transform.DOScale(1f, 0.5f);
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

        EntityComponent currentPlayer = _gameRunner.CurrentPlayer;

        _targetHand = currentPlayer is PlayerComponent ? FindFirstObjectByType<PlayerHand>().transform : FindFirstObjectByType<AIHand>().transform;
        Debug.LogWarning("Container :." + _targetHand.name);

        if (_card == null || currentPlayer == null)
            return;

        if (_gameRunner.TryBuyCard(currentPlayer, _card))
        {
            _card.Build(transform.position, Quaternion.identity, _targetHand);
        }
    }
}
