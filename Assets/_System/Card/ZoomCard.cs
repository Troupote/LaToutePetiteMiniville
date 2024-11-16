using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ZoomCard : MonoBehaviour
{
    [SerializeField]
    private RectTransform _gameObject;
    private float _positionInit;

    public void ZoomOnCard()
    {
        _gameObject.DOKill();

        _positionInit = _gameObject.anchoredPosition.x;
        _gameObject.DOScale(new Vector3(2f, 2f, 1f), 1f);
        _gameObject.DOAnchorPos(new Vector2(_positionInit, -300), 1f);
    }

    public void DeZoomCard()
    {
        _gameObject.DOKill();

        _gameObject.DOScale(new Vector3(1f, 1f, 1f), 1f);
        _gameObject.DOAnchorPos(new Vector2(_positionInit,-100), 0.5f);

    }
}
