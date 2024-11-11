using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{

    #region Fields

    public static SceneLoader Instance;

    [SerializeField]
    private GameObject _loadingScreen;

    [SerializeField]
    private Slider _progressBar;

    private Coroutine _loadingCoroutine;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool LoadScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == sceneName)
            return false;

        if (_loadingCoroutine != null)
            return false;

        _loadingCoroutine = StartCoroutine(LoadSceneAsync(sceneName));

        return true;
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (_loadingScreen != null)
            _loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (_progressBar != null)
                _progressBar.value = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (asyncLoad.progress >= 0.9f)
            {
                // Make a little pause to avoid loading the scene too quickly
                yield return new WaitForSeconds(1f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        if (_loadingScreen != null)
            _loadingScreen.SetActive(false);

        _loadingCoroutine = null;
    }
}
