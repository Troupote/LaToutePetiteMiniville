using DG.Tweening;
using UnityEngine;

public class SlotPlayerCard : MonoBehaviour
{
    [SerializeField]
    private GameObject _printerPrefab;

    [SerializeField]
    private GameObject _positionOfCardBuy;

    [SerializeField]
    private RectTransform _targetPosition;  // Position cible de l'objet

    [SerializeField]
    private float _duration = 1.0f;      // Durée du déplacement

    private MooveCurrentPositionCardHandPlayer _handPlayer;

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

        Vector3 position = _positionOfCardBuy.transform.position;
        GameObject CreatedObject = Instantiate(_printerPrefab, position, Quaternion.identity);

        transform.DOMove(target.position, _duration);
    }



    
}
