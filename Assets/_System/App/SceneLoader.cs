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
    private Animator _transitionAnimator = null;

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

        _transitionAnimator = _loadingScreen.GetComponent<Animator>();
    }

    public bool LoadScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == sceneName)
            return false;

        if (_loadingCoroutine != null)
            return false;

        if (_transitionAnimator == null)
            return false;

        _loadingCoroutine = StartCoroutine(LoadSceneAsync(sceneName));

        return true;
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        _loadingScreen.SetActive(true);
        _progressBar.gameObject.SetActive(false);

        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            AnimatorStateInfo currentStateInfo = _transitionAnimator.GetCurrentAnimatorStateInfo(0);

            // If the animation is in the "Idle" state
            if (currentStateInfo.IsName("Anim_Transition_Idle"))
            {
                if (_progressBar != null && !_progressBar.gameObject.activeSelf)
                    _progressBar.gameObject.SetActive(true);

                // Update progress bar
                if (_progressBar != null)
                    _progressBar.value = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            }

            if (asyncLoad.progress >= 0.9f)
            {
                // Ensure the animation is in the "Idle" state
                while (!_transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("Anim_Transition_Idle"))
                    yield return null;

                // Wait a little for the transition before allowing scene activation
                yield return new WaitForSeconds(0.3f);

                // Allow scene activation
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        if (_progressBar != null && _progressBar.gameObject.activeSelf)
            _progressBar.gameObject.SetActive(false);

        // Start the "fadeIn" transition animation
        _transitionAnimator.SetTrigger("fadeInTrigger");

        // Wait for the end of the "fadeIn" animation before hiding the loading screen
        AnimatorStateInfo stateInfo = _transitionAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        _loadingScreen.SetActive(false);

        _loadingCoroutine = null;
    }
}