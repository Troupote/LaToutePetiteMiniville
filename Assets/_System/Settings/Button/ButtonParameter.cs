using UnityEngine;
using UnityEngine.UI;

public class ButtonParameter : MonoBehaviour
{
    [SerializeField]
    private GameObject _screenParameter;

    [SerializeField]
    private Image _flou;

    public void Pause()
    {
        _screenParameter.gameObject.SetActive(true);
        _flou.gameObject.SetActive(true);
    }

}