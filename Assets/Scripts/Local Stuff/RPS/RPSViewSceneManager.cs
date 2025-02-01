using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RPSViewSceneManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] Button returnButton;
    RectTransform buttonTransform;

    private void Awake()
    {
        returnButton.onClick.AddListener(delegate { SceneController.Instance.GoToScene("MainMenu"); });
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
}
