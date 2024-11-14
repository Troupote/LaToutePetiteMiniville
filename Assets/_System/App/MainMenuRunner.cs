using DG.Tweening;
using UnityEngine;

public class MainMenuRunner : MonoBehaviour
{

    #region Fields
    [SerializeField]
    private AudioSource MenuMusic;

    [SerializeField]
    [Range(0f, 1f)]
    private float MenuVolume;

    [SerializeField]
    private AudioSource ButtonSound;
    #endregion

    #region Public API

    public void Start()
    {
        MenuMusic.volume = 0;
        MenuMusic.Play();
        MenuMusic.DOFade(MenuVolume, 1);
    }

    public void Play()
    {
        Debug.Log("Menu - Play");
        AppManager.Instance.Play();
        ButtonSound.Play();
        MenuMusic.DOFade(0, 1);
    }
    
    public void Credits()
    {
        AppManager.Instance.Credits();
        ButtonSound.Play();
    }
    
    public void Quit()
    {
        AppManager.Instance.Quit();
        ButtonSound.Play();
    }

    #endregion
}
