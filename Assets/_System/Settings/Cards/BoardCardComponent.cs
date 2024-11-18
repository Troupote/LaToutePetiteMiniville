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
        // Trouve GameRunner dans la scène
        _gameRunner = FindFirstObjectByType<GameRunner>();

        if (_card == null)
            Debug.LogError("CardSO is missing", this);

        // Récupère le bouton si ce n'est pas déjà configuré
        if (_cardButton == null)
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
        {
            // Désabonne de l'événement d'achat de carte pour éviter les appels multiples
            _currentPlayer.OnBuyCard.RemoveListener(HandleBuyCard);
            Debug.Log("Listener for OnBuyCard removed.");
        }
    }

    public void SetBoardCard(BuildingSO card)
    {
        // Configure la carte affichée sur le plateau
        _card = card;
        _renderer.sprite = card.Renderer;
        name = _card.name;
    }

    public void HandleCardSelected()
    {
        if (_gameRunner == null)
        {
            Debug.LogError("GameRunner not found in the scene.");
            return;
        }

        // Assigne le joueur courant
        _currentPlayer = _gameRunner.CurrentPlayer;

        // Retire tout écouteur existant pour éviter les appels multiples
        _currentPlayer.OnBuyCard.RemoveListener(HandleBuyCard);

        // Ajoute un écouteur à l'événement d'achat de carte
        _currentPlayer.OnBuyCard.AddListener(HandleBuyCard);

        // Notifie le GameRunner de la carte sélectionnée
        _gameRunner.SelectCardToBuy(_currentPlayer, _card);
    }

    private void HandleBuyCard(EntityComponent player, CardSO card)
    {
        Debug.Log($"BUY! HandleBuyCard called for player: {player.name} and card: {card.Name}");

        // Détermine la main cible en fonction du type de joueur
        _targetHand = _currentPlayer is PlayerComponent
            ? FindFirstObjectByType<PlayerHand>().transform
            : FindFirstObjectByType<AIHand>().transform;

        Debug.LogWarning($"Target Container: {_targetHand.name}");

        // Crée une instance de la carte et l'ajoute à la main du joueur
        CardComponent newCard = card.Build(transform.position, Quaternion.identity, _targetHand);

        _currentPlayer.Cards.Add(newCard);

        // Redimensionne le conteneur pour adapter l'ajout de la nouvelle carte
        ResizeCardContainerComponent container = _targetHand.GetComponent<ResizeCardContainerComponent>();
        if (container != null)
        {
            container.Resize();
        }
    }
}
