using UnityEngine;

public class BackToTheGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _quitScreenParameter;

    public void ReturnGame()
    {
        _quitScreenParameter.gameObject.SetActive(false);
    }

}
