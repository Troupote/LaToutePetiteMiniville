using DG.Tweening;
using UnityEngine;

public class SlotPlayerCard : MonoBehaviour
{
    [Header("Prefabs de cartes")]
    [SerializeField]
    private GameObject _playerCardPrefab; // Prefab pour les cartes du joueur
    [SerializeField]
    private GameObject _aiCardPrefab;    // Prefab pour les cartes de l'IA

    [Header("Position de d�part")]
    [SerializeField]
    private RectTransform _positionOnBoard; // Position de d�part de la carte (plateau)

    [Header("Dur�e de l'animation")]
    [SerializeField]
    private float _slideDuration = 1.0f; // Dur�e de l'animation

    [Header("Mains des joueurs")]
    [SerializeField]
    private RectTransform[] _playerHands; // Tableau des positions des mains des joueurs

    [SerializeField]
    private RectTransform _aiHand;       // Position de la main de l'IA

    private RectTransform _currentTargetHand; // Position actuelle de la main cible
    private EntityComponent _currentEntityComponent; // R�f�rence � l'entit� (joueur ou IA)

    // Assigner l'entit� actuelle (joueur ou IA)
    public void SetEntityComponent(EntityComponent entity)
    {
        _currentEntityComponent = entity;
    }

    /// <summary>
    /// Ajoute une carte sur la sc�ne et l'envoie vers la main cible en fonction de l'entit�.
    /// </summary>
    public void AddCarteOnScene()
    {
        // V�rifie si l'entit� est un joueur ou l'IA
        bool isPlayer = _currentEntityComponent is PlayerComponent;

        // D�terminer le prefab � utiliser en fonction du type d'entit�
        GameObject selectedPrefab = isPlayer ? _playerCardPrefab : _aiCardPrefab;

        // D�terminer la main cible
        if (isPlayer)
        {
            // Pour un joueur, la carte va dans la main du joueur
            _currentTargetHand = _playerHands[0];
        }
        else
        {
            // Pour l'IA, la carte va � la main de l'IA
            _currentTargetHand = _aiHand;
        }

        // Cr�er la carte � la position de d�part
        GameObject createdObject = Instantiate(
            selectedPrefab,
            _positionOnBoard.position,
            Quaternion.identity,
            _positionOnBoard.parent
        );

        // V�rifier que le prefab a un RectTransform
        RectTransform createdCard = createdObject.GetComponent<RectTransform>();
        if (createdCard == null)
        {
            Debug.LogError("Le prefab de carte doit avoir un RectTransform !");
            return;
        }

        // Position initiale de la carte
        createdCard.position = _positionOnBoard.position;

        // Animer la carte vers la main cible
        createdCard.DOMove(_currentTargetHand.position, _slideDuration).SetEase(Ease.InOutQuad);
    }
}
