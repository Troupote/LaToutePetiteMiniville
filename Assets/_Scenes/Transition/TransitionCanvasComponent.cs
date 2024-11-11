using UnityEngine;
using UnityEngine.UI;

public class TransitionCanvasComponent : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
