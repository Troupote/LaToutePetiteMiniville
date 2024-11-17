using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SongController : MonoBehaviour
{
    [SerializeField]
    private Slider _controllerSong;


    public void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        }
        Load();
    }
    public void Volume()
    {
        AudioListener.volume = _controllerSong.value;
        Save();
    }

    private void Load()
    {
        _controllerSong.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume",_controllerSong.value);
    }


}
