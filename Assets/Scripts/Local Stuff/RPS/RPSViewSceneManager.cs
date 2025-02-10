using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Firebase.Database;
using System.Threading.Tasks;

public class RPSViewSceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button returnButton;
    private RectTransform buttonTransform;

    [SerializeField] private Transform playerAArm;
    [SerializeField] private Transform playerBArm;

    [SerializeField] private float padding = -25f;

    private void Awake()
    {
        returnButton.onClick.AddListener(LeaveGame);
        buttonTransform = returnButton.GetComponent<RectTransform>();
    }


    private void Start()
    {
        AlignArmToScreen();

        SetStatusText();
        Invoke(nameof(RevealStatusText), 1.95f);
        Invoke(nameof(RevealButton), 2.05f);
    }


    private void AlignArmToScreen()
    {
        Camera cam = Camera.main;
        float zDepth = Mathf.Abs(cam.transform.position.z - playerAArm.position.z);

        float yPos = cam.WorldToScreenPoint(playerAArm.position).y;

        Vector3 playerAPos = cam.ScreenToWorldPoint(new Vector3(0 + padding, yPos, zDepth));
        Vector3 playerBPos = cam.ScreenToWorldPoint(new Vector3(Screen.width - padding, yPos, zDepth));

        playerAArm.position = playerAPos;
        playerBArm.position = playerBPos;

        AdjustArmScale(zDepth, cam);
    }

    private void AdjustArmScale(float zDepth, Camera cam)
    {
        float screenWidth = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, zDepth)).x * 2;

        float targetWidth = screenWidth;

        float armAWidth = GetArmWidth(playerAArm);
        float armBWidth = GetArmWidth(playerBArm);

        float scaleA = targetWidth / armAWidth;
        float scaleB = targetWidth / armBWidth;

        playerAArm.localScale = new(scaleA, scaleA, 1);
        playerBArm.localScale = new(scaleB, scaleB, 1);
    }

    private float GetArmWidth(Transform arm)
    {
        SpriteRenderer[] sprites = arm.GetComponentsInChildren<SpriteRenderer>();
        if (sprites.Length == 0) return 1f;

        float leftX = float.MaxValue;
        float rightX = float.MinValue;

        foreach (var sprite in sprites)
        {
            Bounds bounds = sprite.bounds;
            leftX = Mathf.Min(leftX, bounds.min.x);
            rightX = Mathf.Max(rightX, bounds.max.x);
        }

        float width = rightX - leftX;
        return Mathf.Max(width, 1f);
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
