using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    [SerializeField] Button RPSButton;
    [SerializeField] Button idleGameButton;
    [SerializeField] Button storeButton;
    [SerializeField] Button profileButton;
    [SerializeField] Button signOutButton;
    [SerializeField] Button watchAdButton;
    [SerializeField] Button cancelButton;
    public GameObject waitingPanel;

    private Button[] buttonsAfterFirstGame;
    private Button[] buttonsAfterWatchedAds;

    private void Awake()
    {
        Instance = this;

        buttonsAfterFirstGame = new Button[]
        {
            storeButton, cancelButton
        };

        buttonsAfterWatchedAds = new Button[]
        {
            watchAdButton, idleGameButton,
        };
    }

    void Start()
    {
        SetUpButtons();
    }


    private void SetUpButtons()
    {
        PlayerData localPlayer = SaveDataManager.Instance.localPlayerData;
        RPSButton.onClick.AddListener(() => RPSMatchMaking.Instance.ConnectToGame());
        profileButton.onClick.AddListener(() => SceneController.Instance.GoToScene("Profile"));
        signOutButton.onClick.AddListener(() => LoginManager.Instance.SignOut());

        if (localPlayer.hasPlayedFirstGame)
        {
            foreach (Button button in buttonsAfterFirstGame)
            {
                button.gameObject.SetActive(true);
            }
            storeButton.onClick.AddListener(() => SceneController.Instance.GoToScene("Store"));
            cancelButton.onClick.AddListener(() => RPSMatchMaking.Instance.CancelMatchMaking());
        }

        if (localPlayer.watchedAdCount != 0)
        {
            for (int i = 0; i < Mathf.Min(localPlayer.watchedAdCount, buttonsAfterWatchedAds.Length); i++)
            {
                buttonsAfterWatchedAds[i].gameObject.SetActive(true);
            }
            watchAdButton.onClick.AddListener(() => SceneController.Instance.GoToScene("AdWatching"));
            idleGameButton.onClick.AddListener(() => SceneController.Instance.GoToScene("IdleGameScene"));
        }
    }


    public async void ToggleWaitingPanel()
    {
        if (waitingPanel.activeSelf && RPSMatchMaking.Instance.gameData != null && RPSMatchMaking.Instance.gameData.gameID != null)
        {
            try
            {
                await RPSMatchMaking.Instance.RemoveWaitingGame();
            }
            catch { }
        }

        waitingPanel.SetActive(!waitingPanel.activeSelf);
    }
}