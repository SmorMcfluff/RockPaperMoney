using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RPSMenu : MonoBehaviour
{
    [SerializeField] Button rockButton;
    [SerializeField] Button paperButton;
    [SerializeField] Button scissorsButton;
    [SerializeField] Button readyButton;

    private void Awake()
    {
        rockButton.onClick.AddListener(delegate { SetHandSign(HandSign.Rock); });
        paperButton.onClick.AddListener(delegate { SetHandSign(HandSign.Paper); });
        scissorsButton.onClick.AddListener(delegate { SetHandSign(HandSign.Scissors); });
    }

    public void SetHandSign(HandSign handSign)
    {
        RPSOnlineManager.Instance.localPlayer.handSign = handSign;
    }

    public void DeclareReady()
    {
        if(RPSOnlineManager.Instance.localPlayer.isPlayerA)
        {
            RPSOnlineManager.Instance.gameData.playerAReady = true;
        }
        else
        {
            RPSOnlineManager.Instance.gameData.playerBReady = true;
        }

        FirebaseDatabase.DefaultInstance.RootReference.Child("games").Child(RPSOnlineManager.Instance.gameData.gameID);
    }
}
