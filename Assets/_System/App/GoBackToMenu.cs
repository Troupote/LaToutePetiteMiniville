using UnityEngine;

public class GoBackToMenu : MonoBehaviour
{
    public void BackToMenu()
    {
        AppManager.Instance.Menu();
    }
}
