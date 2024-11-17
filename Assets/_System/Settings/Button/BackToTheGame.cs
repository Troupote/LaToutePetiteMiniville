using UnityEngine;
using UnityEngine.UI;

public class BackToTheGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _quitScreenParameter;

    [SerializeField]
    private Image _flou;

    public void ReturnGame()
    {
        _quitScreenParameter.gameObject.SetActive(false);
        _flou.gameObject.SetActive(false);

    }

}
