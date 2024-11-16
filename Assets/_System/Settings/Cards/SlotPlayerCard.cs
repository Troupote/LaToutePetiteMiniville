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

    [Header("Position de d�part")]
    [SerializeField]
    private RectTransform _positionOnBoard; // Position de d�part de la carte (plateau)

    [Header("Dur�e de l'animation")]
    [SerializeField]
    private float _slideDuration = 1.0f; // Dur�e de l'animation

    [Header("Mains des joueurs")]
    [SerializeField]
    private RectTransform _playerHands; // Tableau des positions des mains des joueurs
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
        // V�rifie si l'entit� est un joueur
        bool isPlayer = _currentEntityComponent is PlayerComponent;

        // D�terminer le prefab � utiliser en fonction du type d'entit�
        GameObject selectedPrefab = isPlayer ? _playerCardPrefab : _aiCardPrefab;

        // Si c'est un joueur, v�rifier la taille de sa main
        if (isPlayer)
        {
            // V�rifier si la main du joueur n'a pas d�j� 2 cartes
            if (_currentEntityComponent.Cards.Count >= 2)
            {
                Debug.Log("La main du joueur est d�j� pleine avec 2 cartes.");
                return; // La main est pleine, ne pas ajouter de carte
            }

            // D�terminer la main cible du joueur
            // Comme on ne g�re pas l'index du joueur ici, on r�cup�re simplement la premi�re main disponible
            _currentTargetHand = _playerHands; // Choisir la premi�re main du tableau (_playerHands[0] par exemple)
        }
        else
        {
            // Pour l'IA, la carte va dans la main de l'IA
            _currentTargetHand = _aiHand;
        }

        // Cr�er la carte � la position de d�part
        GameObject createdObject = Instantiate(
            selectedPrefab,
            _positionOnBoard.position, // Position de d�part sur le plateau
            Quaternion.identity,
            _positionOnBoard
        );

        // V�rifier que le prefab a un RectTransform
        RectTransform createdCard = createdObject.GetComponent<RectTransform>();

        // Position initiale de la carte
        createdCard.position = _positionOnBoard.position;

        // Animer la carte vers la main cible
        //createdCard.DOMove(_currentTargetHand.position, _slideDuration).SetEase(Ease.InOutQuad);

        // Ajouter la carte � la main du joueur
        if (isPlayer)
        {
            Destroy( createdObject );
            // Cr�er un GameObject pour la carte � ajouter � la main du joueur
            GameObject cardObject = Instantiate(_playerCardPrefab, _currentTargetHand.position, Quaternion.identity, _playerHands);
        }
        else
        {
            Destroy(createdObject);
            // Pour l'IA, on instancie une carte de mani�re similaire
            GameObject cardObject = Instantiate(_aiCardPrefab, _currentTargetHand.position, Quaternion.identity,_aiHand );
        }
    }
}
