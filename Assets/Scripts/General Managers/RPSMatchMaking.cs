/*
 * Special thanks to  Salmaster for letting
 * me take a look at his matchmaking code.
 * I couldn't have done it without you.
 */

using Firebase.Database;
using Firebase.Extensions;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RPSMatchMaking : MonoBehaviour
{
    public static RPSMatchMaking Instance;
    bool gameIsStarting = false;

    FirebaseDatabase db;

    public RPSGameData gameData;
    public RPSPlayer localPlayer;

    public delegate void OnLoadedDelegate(DataSnapshot snap);
    OnLoadedDelegate onLoaded;


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

        db = FirebaseDatabase.DefaultInstance;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            ConnectToGame();
    }


    public void ConnectToGame()
    {
        gameIsStarting = false;
        LoadGames(onLoaded);
    }


    private void LoadGames(OnLoadedDelegate onLoadedDelegate)
    {
        db.RootReference.Child("waitingGames").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            DataSnapshot snap = task.Result;

            CheckLoadedGames(snap);
        });
    }


    private void CheckLoadedGames(DataSnapshot snap)
    {
        var snapshots = snap.Children;
        List<RPSGameData> rpsGames = new();

        foreach (var shot in snapshots)
        {
            RPSGameData rpsGame = JsonUtility.FromJson<RPSGameData>(shot.GetValue(true).ToString());
            rpsGames.Add(rpsGame);
        }

        bool managedToConnect = false;
        foreach (var rpsGame in rpsGames)
        {
            if (!rpsGame.playerBConnected)
            {
                gameData = rpsGame;

                localPlayer = new RPSPlayer()
                {
                    equippedSkin = SaveDataManager.Instance.localPlayerData.equippedSkin
                };

                rpsGame.playerBJson = JsonUtility.ToJson(localPlayer);
                rpsGame.playerBConnected = true;
                db.RootReference.Child("waitingGames").Child(rpsGame.gameID).SetValueAsync(JsonUtility.ToJson(rpsGame)).ContinueWithOnMainThread(task =>
                {
                    if (task.Exception != null)
                        Debug.LogWarning(task.Exception);
                });

                managedToConnect = true;
                break;
            }
        }

        if (managedToConnect)
        {
            SceneController.Instance.GoToScene("RPSScene");
        }
        else
        {
            CreateGame();
        }
    }


    public void CreateGame()
    {
        MainMenuManager.Instance.ToggleWaitingPanel();

        localPlayer = new RPSPlayer()
        {
            equippedSkin = SaveDataManager.Instance.localPlayerData.equippedSkin,
            isPlayerA = true
        };

        var rpsGame = new RPSGameData(localPlayer);
        var rpsGameJson = JsonUtility.ToJson(rpsGame);

        gameData = rpsGame;

        db.RootReference.Child("waitingGames").Child(gameData.gameID).SetValueAsync(rpsGameJson).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }

        });
        db.RootReference.Child("waitingGames").Child(gameData.gameID).ValueChanged += ValueChanged;
    }


    private void ValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (!gameIsStarting)
        {
            db.RootReference.Child("waitingGames").Child(gameData.gameID).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogWarning(task.Exception);
                    return;
                }

                var snap = task.Result;

                if (!snap.Exists)
                {
                    Debug.LogWarning("Game data does not exist anymore!");
                    return;
                }

                var snapJson = Regex.Unescape(snap.GetRawJsonValue()).Trim('"');
                var data = JsonUtility.FromJson<RPSGameData>(snapJson);


                if (data.playerAConnected && data.playerBConnected)
                {
                    Debug.Log("Both players are connected! Starting game...");
                    StartGame();
                }
            });
        }
    }


    private async void StartGame()
    {
        gameIsStarting = true;
        Debug.Log("Starting Game");

        db.RootReference.Child("waitingGames").Child(gameData.gameID).ValueChanged -= ValueChanged;

        string playerLetter = localPlayer.GetPlayerLetter();

        var checkGameTask = await db.RootReference.Child("waitingGames").Child(gameData.gameID).GetValueAsync();
        if (checkGameTask.Exists)
        {
            try
            {
                await RemoveWaitingGame();
            }
            catch { }
        }

        try
        {
            await db.RootReference.Child("games").Child(gameData.gameID).Child(playerLetter).SetValueAsync(localPlayer);
        }
        catch { }

        SceneController.Instance.GoToScene("RPSScene");
    }


    public async Task RemoveWaitingGame()
    {
        await db.RootReference.Child("waitingGames").Child(gameData.gameID).RemoveValueAsync();
    }


    private async void OnApplicationFocus(bool focus)
    {
        if (MainMenuManager.Instance != null && MainMenuManager.Instance.waitingPanel.activeSelf)
        {
            MainMenuManager.Instance.ToggleWaitingPanel();
        }

        if (gameData != null && !string.IsNullOrEmpty(gameData.gameID))
        {
            await RemoveWaitingGame();
        }
    }

    private async void OnApplicationQuit()
    {
        if (gameData != null && !string.IsNullOrEmpty(gameData.gameID))
        {
            await RemoveWaitingGame();

        }
    }
}
