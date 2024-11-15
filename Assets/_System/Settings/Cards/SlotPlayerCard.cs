using DG.Tweening;
using UnityEngine;

public class SlotPlayerCard : MonoBehaviour
{
    [SerializeField]
    private GameObject _printerPrefab;

    [SerializeField]
    private GameObject _positionOfCardBuy;

    [SerializeField] private Transform _targetPosition;  // Position cible de l'objet
    [SerializeField] private float _duration = 1.0f;      // Durée du déplacement

    public void AddCarteOnScene() 
    {
        Vector3 position = _positionOfCardBuy.transform.position;
        GameObject CreatedObject = Instantiate(_printerPrefab, position, Quaternion.identity);
    }

    /*
    public void MooveCardInHandPlayer() 
    {
       transform.DOMove(new Vector3(
    }
    */
}
