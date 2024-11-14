using DG.Tweening;
using UnityEngine;

public class MainMenuRunner : MonoBehaviour
{

    #region Fields
    [SerializeField]
    private AudioSource MenuMusic;
    #endregion

    #region Public API

    public void Start()
    {
        MenuMusic.volume = 0;
        MenuMusic.Play();
        MenuMusic.DOFade(1, 1);
    }

    public void Play()
    {
        Debug.Log("Menu - Play");
        AppManager.Instance.Play();
        MenuMusic.DOFade(0, 1);
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
