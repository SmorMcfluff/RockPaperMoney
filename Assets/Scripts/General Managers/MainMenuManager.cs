using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button RPSButton;
    [SerializeField] Button idleGameButton;
    [SerializeField] Button storeButton;
    [SerializeField] Button profileButton;


    void Start()
    {
        RPSButton.onClick.AddListener(delegate { SceneController.Instance.GoToScene(""); });
        idleGameButton.onClick.AddListener(delegate { SceneController.Instance.GoToScene("IdleGameScene"); });
        storeButton.onClick.AddListener(delegate { SceneController.Instance.GoToScene(""); });
        profileButton.onClick.AddListener(delegate { LoginManager.Instance.SignOut();/*SceneController.Instance.GoToScene("");*/ });
    }
}
