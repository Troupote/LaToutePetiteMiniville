using UnityEngine;

public class MainMenuRunner : MonoBehaviour
{

    #region Fields
    #endregion

    #region Public API

    public void Play()
    {
        Debug.Log("Menu - Play");
        AppManager.Instance.Play();
    }
    
    public void Credits()
    {
        AppManager.Instance.Credits();
    }
    
    public void Quit()
    {
        AppManager.Instance.Quit();
    }

    #endregion
}
