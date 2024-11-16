using UnityEngine;

public class ButtonParameter : MonoBehaviour
{
    [SerializeField]
    private GameObject ScreenParameter;

    public void Pause()
    {
        ScreenParameter.gameObject.SetActive(true);
    }

}