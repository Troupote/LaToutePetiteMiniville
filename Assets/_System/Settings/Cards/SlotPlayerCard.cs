using DG.Tweening;
using UnityEngine;

public class SlotPlayerCard : MonoBehaviour
{
    [SerializeField]
    private GameObject _printerPrefab;

    [SerializeField]
    private RectTransform _positionOnBoard;  


    [SerializeField]
    private RectTransform _playerHandPosition;  

    [SerializeField]
    private float _duration = 1.0f;      // Durée du déplacement

    private EntityComponent _entityComponent;



    Transform target=null;  

    public void SetPlayer(EntityComponent player)
    {
        _entityComponent = player;
    }

    public void AddCarteOnScene() 
    {
        if (_entityComponent is PlayerComponent)
            target.position = FindFirstObjectByType<PlayerDeckContainer>().transform.position;

        GameObject CreatedObject = Instantiate(_printerPrefab, _positionOnBoard.position, Quaternion.identity,_positionOnBoard.parent);

        // Récupérer le RectTransform de la carte
        RectTransform CreatedCard = CreatedObject.GetComponent<RectTransform>();

        CreatedCard.position = _positionOnBoard.position;

        CreatedCard.DOMove(_playerHandPosition.position, _duration).SetEase(Ease.InOutQuad).onComplete();
    }
}
