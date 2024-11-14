using UnityEngine;

public class ButtonParameter : MonoBehaviour
{
    public GameObject ScreenParameter;

    public ButtonParameter( GameObject screenParameter)
    {
        ScreenParameter = screenParameter;
    }

    public void ScreenVisibility()
    {
        if (ScreenParameter.gameObject.activeSelf != false)
        {
            ScreenParameter.gameObject.SetActive(false);
        }
        else 
        { 
            ScreenParameter.gameObject.SetActive(true);
        }
    }
}
