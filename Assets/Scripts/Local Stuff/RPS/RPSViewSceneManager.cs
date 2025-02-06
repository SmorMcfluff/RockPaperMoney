using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Firebase.Database;
using System.Threading.Tasks;

public class RPSViewSceneManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] Button returnButton;
    RectTransform buttonTransform;

    private void Awake()
    {
        returnButton.onClick.AddListener(delegate { LeaveGame(); });
        buttonTransform = returnButton.GetComponent<RectTransform>();
    }
    void Start()
    {
        SetStatusText();
        Invoke(nameof(RevealStatusText), 1.95f);
        Invoke(nameof(RevealButton), 2.05f);
    }

    private void SetStatusText()
    {
        var gameResult = RPSMatchMaking.Instance.gameData.gameResult;

        string status = gameResult switch
        {
            GameResult.LocalWin => "You Won!",
            GameResult.OnlineWin => "You Lost!",
            GameResult.Tie => "Tie!",
            _ => "Something odd happened here",
        };
        statusText.text = status;
    }

    private void RevealStatusText()
    {
        statusText.rectTransform.DOAnchorPosY(-50, 0.5f);
    }

    private void RevealButton()
    {
        buttonTransform.DOAnchorPosY(50, 0.5f);
    }

    private async void LeaveGame()
    {
        await RemoveGame();

        SceneController.Instance.GoToScene("MainMenu");
    }

    private async Task RemoveGame()
    {
        FirebaseDatabase db = FirebaseDatabase.DefaultInstance;
        var gameData = RPSMatchMaking.Instance.gameData;

        await db.RootReference.Child("games").Child(gameData.gameID).RemoveValueAsync();
    }

    
    private async void OnApplicationFocus(bool focus)
    {
        await RemoveGame();
    }
}
