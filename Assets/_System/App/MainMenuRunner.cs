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
    private AudioSource PlayButtonSound;

    [SerializeField]
    private AudioSource QuitButtonSound;

    [SerializeField]
    private AudioSource CreditButtonSound;
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
        PlayButtonSound.Play();
        MenuMusic.DOFade(0, 1);
    }
    
    public void Credits()
    {
        CreditTextMove.direction = "up";
        ButtonMovement.direction = "down";
        CreditButtonSound.Play();
    }
    
    public void Leave_Credits()
    {
        CreditTextMove.direction = "down";
        ButtonMovement.direction = "up";
        QuitButtonSound.Play();
    }

    public void Quit()
    {
        AppManager.Instance.Quit();
        QuitButtonSound.Play();
    }

    #endregion
}
