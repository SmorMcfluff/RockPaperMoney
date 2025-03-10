using Firebase.Database;
using Firebase.Extensions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissors : MonoBehaviour
{

    float timer = 0;
    float roundMaxLength = 15f;

    Color defaultBgImgColor;

    Button[] handsignButtons;
    [SerializeField] Image[] handSignBackgroundImgs;
    [SerializeField] Button rockButton;
    [SerializeField] Button paperButton;
    [SerializeField] Button scissorsButton;

    [SerializeField] Button readyButton;

    [SerializeField] TextMeshProUGUI statusText;

    FirebaseDatabase db;


    private void Awake()
    {
        db = FirebaseDatabase.DefaultInstance;

        handsignButtons = new Button[] { rockButton, paperButton, scissorsButton };

        AddButtonListeners();
        SetHandSignButtons();
        UpdatePlayerOnFirebase();
        Subscribe();

        defaultBgImgColor = handSignBackgroundImgs[0].color;
    }


    public void Update()
    {
        Timer();
    }


    private void AddButtonListeners()
    {
        rockButton.onClick.AddListener(() => SetHandSign(HandSign.Rock, 0));
        paperButton.onClick.AddListener(() => SetHandSign(HandSign.Paper, 1));
        scissorsButton.onClick.AddListener(() => SetHandSign(HandSign.Scissors, 2));

        readyButton.onClick.AddListener(() => DeclareReady());
    }


    private void SetHandSignButtons()
    {
        SkinType equippedSkin = SaveDataManager.Instance.localPlayerData.equippedSkin;
        Sprite[] icons = SkinManager.Instance.GetIcons(equippedSkin);

        var unlockedHandSigns = SaveDataManager.Instance.localPlayerData.unlockedHandSigns;
        for (int i = 0; i < 3; i++)
        {
            if (icons.Length > 0)
            {
                handsignButtons[i].image.sprite = icons[i];
            }


            if (!unlockedHandSigns[i])
            {
                handsignButtons[i].interactable = false;
                handsignButtons[i].image.color = Color.gray;
                handSignBackgroundImgs[i].color = Color.gray;
            }
        }
    }

    private void Timer()
    {
        if (SaveDataManager.Instance.localPlayerData.hasPlayedFirstGame)
        {
            timer += Time.deltaTime;
            if (timer >= roundMaxLength)
            {
                EndGame();
            }
        }
    }


    public void SetHandSign(HandSign handSign, int buttonIndex)
    {
        if (!RPSMatchMaking.Instance.localPlayer.isReady)
        {
            RPSMatchMaking.Instance.localPlayer.handSign = handSign;

            var unlockedHandSigns = SaveDataManager.Instance.localPlayerData.unlockedHandSigns;
            for (int i = 0; i < 3; i++)
            {
                if (i == buttonIndex)
                {
                    handSignBackgroundImgs[i].color = Color.green;
                }
                else if (unlockedHandSigns[i])
                {
                    handsignButtons[i].image.color = Color.white;
                    handSignBackgroundImgs[i].color = defaultBgImgColor;
                }
                else
                {
                    handSignBackgroundImgs[i].color = defaultBgImgColor;
                }
            }
        }
    }


    private async void UpdatePlayerOnFirebase()
    {
        var localPlayer = JsonUtility.ToJson(RPSMatchMaking.Instance.localPlayer);
        var gameID = RPSMatchMaking.Instance.gameData.gameID;
        var playerLetter = RPSMatchMaking.Instance.localPlayer.GetPlayerLetter();

        await db.RootReference.Child("games").Child(gameID).Child(playerLetter).SetValueAsync(localPlayer).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);
        });

        await UpdatePlayersLocally();
    }

    private async Task UpdatePlayersLocally()
    {
        var currentGame = RPSMatchMaking.Instance.gameData;

        currentGame.playerA = await GetPlayerData("playerA");
        currentGame.playerB = await GetPlayerData("playerB");
    }


    private void Subscribe()
    {
        var gameID = RPSMatchMaking.Instance.gameData.gameID;

        db.RootReference.Child("games").Child(gameID).ValueChanged -= ValueChanged;
        db.RootReference.Child("games").Child(gameID).ValueChanged += ValueChanged;
    }

    private void Unsubscribe()
    {
        var gameID = RPSMatchMaking.Instance.gameData.gameID;

        db.RootReference.Child("games").Child(gameID).ValueChanged -= ValueChanged;
    }


    private bool gameHasEnded;
    private async void ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (gameHasEnded) return;

        await UpdatePlayersLocally();

        bool playerDisconnected = CheckPlayerConnection();
        if (PlayersAreReady() || playerDisconnected)
        {
            gameHasEnded = true;
            Unsubscribe();
            EndGame(playerDisconnected);
        }
    }


    private bool PlayersAreReady()
    {
        var currentGame = RPSMatchMaking.Instance.gameData;

        return (currentGame.playerA != null && currentGame.playerB != null
            && currentGame.playerA.isReady && currentGame.playerB.isReady);
    }


    private bool CheckPlayerConnection()
    {
        var currentGame = RPSMatchMaking.Instance.gameData;
        bool playerDisconnected = JsonUtility.ToJson(currentGame).Contains("DISCONNECTED");

        if (playerDisconnected)
        {
            statusText.text = "Opponent disconnected";
        }

        return playerDisconnected;
    }


    private void EndGame(bool playerDisconnected = false)
    {
        var gameData = RPSMatchMaking.Instance.gameData;

        var players = SetLocalPlayer();
        PlayerData localPlayer = SaveDataManager.Instance.localPlayerData;
        RPSPlayer localRPSPlayer = players[0];
        RPSPlayer onlineRPSPlayer = players[1];

        GameResult gameResult = CompareHandSigns(localRPSPlayer, onlineRPSPlayer);

        if (localPlayer.hasPlayedFirstGame)
        {
            switch (gameResult)
            {
                case GameResult.LocalWin:
                    localPlayer.winCount++;
                    break;
                case GameResult.OnlineWin:
                    localPlayer.lossCount++;
                    break;
            }
        }
        else
        {
            localPlayer.hasPlayedFirstGame = true;
            InternetChecker.IsInternetAvailable(() => SaveDataManager.Instance.SavePlayer());
        }


        gameData.gameResult = gameResult;

        var isPlayerA = localRPSPlayer.GetPlayerLetter() == "playerA";

        gameData.playerA = isPlayerA ? localRPSPlayer : onlineRPSPlayer;
        gameData.playerB = isPlayerA ? onlineRPSPlayer : localRPSPlayer;


        if (!playerDisconnected)
        {
            InternetChecker.IsInternetAvailable(() => SaveDataManager.Instance.SavePlayer());
            SceneController.Instance.GoToScene("RPSView");
        }
        else
        {
            statusText.text = "Your opponent disconnected!";
            readyButton.gameObject.SetActive(false);
        }
    }


    private RPSPlayer[] SetLocalPlayer()
    {
        RPSPlayer[] players = new RPSPlayer[2];

        var currentGame = RPSMatchMaking.Instance.gameData;

        if (RPSMatchMaking.Instance.localPlayer.isPlayerA)
        {
            players[0] = currentGame.playerA;
            players[1] = currentGame.playerB;
        }
        else
        {
            players[0] = currentGame.playerB;
            players[1] = currentGame.playerA;
        }

        return players;
    }


    private GameResult CompareHandSigns(RPSPlayer localPlayer, RPSPlayer onlinePlayer)
    {
        if (localPlayer.handSign == onlinePlayer.handSign)
        {
            return GameResult.Tie;
        }

        if (localPlayer.handSign == HandSign.Undecided || onlinePlayer.handSign == HandSign.Undecided)
        {
            return GameResult.Tie;
        }

        return (localPlayer.handSign, onlinePlayer.handSign) switch
        {
            (HandSign.Rock, HandSign.Scissors) => GameResult.LocalWin,
            (HandSign.Rock, HandSign.Paper) => GameResult.OnlineWin,
            (HandSign.Paper, HandSign.Rock) => GameResult.LocalWin,
            (HandSign.Paper, HandSign.Scissors) => GameResult.OnlineWin,
            (HandSign.Scissors, HandSign.Paper) => GameResult.LocalWin,
            (HandSign.Scissors, HandSign.Rock) => GameResult.OnlineWin,
            _ => GameResult.Tie
        };
    }


    private void DeclareReady()
    {
        if (!RPSMatchMaking.Instance.localPlayer.isReady && RPSMatchMaking.Instance.localPlayer.handSign != HandSign.Undecided)
        {
            readyButton.image.color = ColorHelper.CombineColors(Color.green, Color.black);
            RPSMatchMaking.Instance.localPlayer.isReady = true;
        }

        UpdatePlayerOnFirebase();
    }


    private async Task<RPSPlayer> GetPlayerData(string playerLetter)
    {
        var gameID = RPSMatchMaking.Instance.gameData.gameID;

        try
        {
            var dataSnapshot = await db.RootReference.Child("games").Child(gameID).Child(playerLetter).GetValueAsync();

            if (dataSnapshot.Exists)
            {
                string jsonData = Regex.Unescape(dataSnapshot.GetRawJsonValue()).Trim('"');

                return JsonUtility.FromJson<RPSPlayer>(jsonData);
            }
        }
        catch
        {
        }

        return new RPSPlayer();
    }

    private async void OnApplicationFocus(bool focus)
    {
        var gameID = RPSMatchMaking.Instance.gameData.gameID;
        var playerLetter = RPSMatchMaking.Instance.localPlayer.GetPlayerLetter();
        await db.RootReference.Child("games").Child(gameID).Child(playerLetter).SetValueAsync("DISCONNECTED").ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);
        });
    }
}

public enum GameResult
{
    LocalWin,
    OnlineWin,
    Tie
}