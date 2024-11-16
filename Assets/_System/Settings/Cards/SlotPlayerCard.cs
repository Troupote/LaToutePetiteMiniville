using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class SlotPlayerCard : MonoBehaviour
{
    [Header("Prefabs de cartes")]
    [SerializeField]
    private GameObject _playerCardPrefab; // Prefab pour les cartes du joueur
    [SerializeField]
    private GameObject _aiCardPrefab;    // Prefab pour les cartes de l'IA

    [Header("Position de départ")]
    [SerializeField]
    private RectTransform _positionOnBoard; // Position de départ de la carte (plateau)

    [Header("Durée de l'animation")]
    [SerializeField]
    private float _slideDuration = 1.0f; // Durée de l'animation

    [Header("Mains des joueurs")]
    [SerializeField]
    private RectTransform _playerHands; // Tableau des positions des mains des joueurs
    [SerializeField]
    private RectTransform _aiHand;       // Position de la main de l'IA

    private RectTransform _currentTargetHand; // Position actuelle de la main cible
    private EntityComponent _currentEntityComponent; // Référence à l'entité (joueur ou IA)

    // Assigner l'entité actuelle (joueur ou IA)
    public void SetEntityComponent(EntityComponent entity)
    {
        _currentEntityComponent = entity;
    }

    /// <summary>
    /// Ajoute une carte sur la scène et l'envoie vers la main cible en fonction de l'entité.
    /// </summary>
    public void AddCarteOnScene()
    {
        // Vérifie si l'entité est un joueur
        bool isPlayer = _currentEntityComponent is PlayerComponent;

        // Déterminer le prefab à utiliser en fonction du type d'entité
        GameObject selectedPrefab = isPlayer ? _playerCardPrefab : _aiCardPrefab;

        // Si c'est un joueur, vérifier la taille de sa main
        if (isPlayer)
        {
            // Vérifier si la main du joueur n'a pas déjà 2 cartes
            if (_currentEntityComponent.Cards.Count >= 2)
            {
                Debug.Log("La main du joueur est déjà pleine avec 2 cartes.");
                return; // La main est pleine, ne pas ajouter de carte
            }

            // Déterminer la main cible du joueur
            // Comme on ne gère pas l'index du joueur ici, on récupère simplement la première main disponible
            _currentTargetHand = _playerHands; // Choisir la première main du tableau (_playerHands[0] par exemple)
        }
        else
        {
            // Pour l'IA, la carte va dans la main de l'IA
            _currentTargetHand = _aiHand;
        }

        // Créer la carte à la position de départ
        GameObject createdObject = Instantiate(
            selectedPrefab,
            _positionOnBoard.position, // Position de départ sur le plateau
            Quaternion.identity,
            _positionOnBoard
        );

        // Vérifier que le prefab a un RectTransform
        RectTransform createdCard = createdObject.GetComponent<RectTransform>();

        // Position initiale de la carte
        createdCard.position = _positionOnBoard.position;

        // Animer la carte vers la main cible
        //createdCard.DOMove(_currentTargetHand.position, _slideDuration).SetEase(Ease.InOutQuad);

        // Ajouter la carte à la main du joueur
        if (isPlayer)
        {
            Destroy( createdObject );
            // Créer un GameObject pour la carte à ajouter à la main du joueur
            GameObject cardObject = Instantiate(_playerCardPrefab, _currentTargetHand.position, Quaternion.identity, _playerHands);
        }
        else
        {
            Destroy(createdObject);
            // Pour l'IA, on instancie une carte de manière similaire
            GameObject cardObject = Instantiate(_aiCardPrefab, _currentTargetHand.position, Quaternion.identity,_aiHand );
        }
    }
}
