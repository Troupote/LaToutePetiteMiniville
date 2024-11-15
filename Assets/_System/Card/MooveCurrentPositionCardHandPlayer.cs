using Unity.VisualScripting;
using UnityEngine;

public class MooveCurrentPositionCardHandPlayer : MonoBehaviour
{
    private RectTransform _nextPosition;
    public void CurrentPosition()
    {
        _nextPosition.anchoredPosition += Vector2.right * 136;
    }
}
