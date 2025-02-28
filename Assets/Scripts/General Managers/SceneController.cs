using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    private string lastScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void GoToScene(string sceneName)
    {
        lastScene = SceneManager.GetActiveScene().name;
        InternetChecker.IsInternetAvailable(() => SaveDataManager.Instance.SavePlayer());
        SceneManager.LoadScene(sceneName);
        SetOrientation(sceneName);
    }

    public void ReturnToLastScene()
    {
        if (!SaveDataManager.Instance.localPlayerData.hasFinishedTutorial)
        {
            GoToScene("MainMenu");
        }
        else
        {
            GoToScene(lastScene);
        }
    }


    private void SetOrientation(string sceneName)
    {
        if (sceneName.Contains("RPS") && Screen.orientation != ScreenOrientation.LandscapeLeft)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            return;
        }

        if (!sceneName.Contains("RPS") && Screen.orientation != ScreenOrientation.Portrait)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }
}
