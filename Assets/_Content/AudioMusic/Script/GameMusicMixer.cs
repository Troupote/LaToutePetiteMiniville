using DG.Tweening;
using UnityEngine;

public class GameMusicMixer : MonoBehaviour
{
    [SerializeField]
    private AudioSource MusicSource = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MusicSource.volume = 0;
        MusicSource.Play();
        MusicSource.DOFade(0.6f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
