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

    [Header("Mains de l'ia")]
    [SerializeField]
    private RectTransform _aiHand;       // Position de la main de l'IA

    [SerializeField]
    private ResizeSpaceCard _sizeCardIa;
    
    [SerializeField]
    private ResizeSpaceCard _sizeCardPlayer;

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
            _currentTargetHand = _playerHands; 
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
            GameObject cardObject = Instantiate(_playerCardPrefab, _currentTargetHand.position, Quaternion.identity, _playerHands);

            _sizeCardPlayer.Resize();



        }
        else
        {
            Destroy(createdObject);
            // Pour l'IA, on instancie une carte de manière similaire
            GameObject cardObject = Instantiate(_aiCardPrefab, _currentTargetHand.position, Quaternion.identity, _aiHand);
            _sizeCardIa.Resize();


        }

    }
}
