using UnityEngine;

/// <summary>
/// Manages the overall game state and scene transitions.
/// </summary>
public class AppManager : MonoBehaviour
{
    #region Nested

    /// <summary>
    /// Represents the different game states.
    /// </summary>
    public enum AppState
    {
        None,
        MainMenu,
        Game,
        Credits
    }

    #endregion

    #region Fields

    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static AppManager Instance;

    private const string _mainMenuSceneName = "SC_MainMenu";

    /// <summary>
    /// The current <see cref="AppState"/>.
    /// </summary>
    private AppState _currentAppState;


    #endregion

    /// <summary>
    /// Ensures only one instance of GameManager exists using Singleton.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (_currentAppState == AppState.None)
            _currentAppState = AppState.MainMenu;

        ChangeState(AppState.MainMenu);
    }

    /// <summary>
    /// Changes the game state and loads the corresponding scene.
    /// </summary>
    /// <param name="targetState">The new game state.</param>
    private void ChangeState(AppState targetState)
    {
        _currentAppState = targetState;

        switch (_currentAppState)
        {
            case AppState.MainMenu:
                SceneLoader.Instance.LoadScene(_mainMenuSceneName);
                break;
            case AppState.Game:
                SceneLoader.Instance.LoadScene("CityView");
                break;
            case AppState.Credits:
                SceneLoader.Instance.LoadScene("BuildingMenu");
                break;
            default:
                Debug.LogError("Error ! Invalid application state.");
                break;
        }
    }
}
