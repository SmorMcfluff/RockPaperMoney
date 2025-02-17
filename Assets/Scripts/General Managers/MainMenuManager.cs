using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    [SerializeField] Button RPSButton;
    [SerializeField] Button idleGameButton;
    [SerializeField] Button storeButton;
    [SerializeField] Button profileButton;
    [SerializeField] Button signOutButton;
    public GameObject waitingPanel;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RPSButton.onClick.AddListener(delegate { RPSMatchMaking.Instance.ConnectToGame(); });
        idleGameButton.onClick.AddListener(delegate { SceneController.Instance.GoToScene("IdleGameScene"); });
        storeButton.onClick.AddListener(delegate { SceneController.Instance.GoToScene("Store"); });
        profileButton.onClick.AddListener(delegate { SceneController.Instance.GoToScene("Profile"); });
        signOutButton.onClick.AddListener(delegate { LoginManager.Instance.SignOut(); });
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

            var gameID = string.Empty;
        }

        waitingPanel.SetActive(!waitingPanel.activeSelf);
    }
}