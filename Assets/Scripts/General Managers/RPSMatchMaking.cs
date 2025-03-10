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
using Firebase.Auth;
using System;
using System.Linq;
using UnityEngine.InputSystem;

public class RPSMatchMaking : MonoBehaviour
{
    public static RPSMatchMaking Instance;
    bool gameIsStarting = false;

    FirebaseDatabase db;
    FirebaseAuth auth;
    public string userId;

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
        auth = FirebaseAuth.DefaultInstance;
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
            CheckLoadedGames(task.Result);
        });
    }


    private void CheckLoadedGames(DataSnapshot snap)
    {
        if (!SaveDataManager.Instance.localPlayerData.hasFinishedTutorial)
        {
            StartOfflineGame();
            return;
        }
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
            if (!rpsGame.playerBConnected && rpsGame.hostUserId != userId)
            {
                gameData = rpsGame;

                localPlayer = new RPSPlayer()
                {
                    playerData = SaveDataManager.Instance.localPlayerData,
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
            playerData = SaveDataManager.Instance.localPlayerData,
            equippedSkin = SaveDataManager.Instance.localPlayerData.equippedSkin,
            isPlayerA = true
        };

        userId = auth.CurrentUser.UserId;
        var rpsGame = new RPSGameData(localPlayer, userId);
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
                    StartGame();
                }
            });
        }
    }


    private async void StartGame()
    {
        gameIsStarting = true;

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


    public async void StartOfflineGame()
    {
        localPlayer = new RPSPlayer()
        {
            playerData = SaveDataManager.Instance.localPlayerData,
            equippedSkin = SaveDataManager.Instance.localPlayerData.equippedSkin,
            isPlayerA = true
        };

        int unlockedSign = Array.IndexOf(SaveDataManager.Instance.localPlayerData.unlockedHandSigns, true) + 1;
        int aiSign = unlockedSign + 1;
        if (aiSign > 3) aiSign = 1;

        gameData = new RPSGameData(localPlayer, userId)
        {
            playerB = new RPSPlayer()
            {
                playerData = new PlayerData(),
                handSign = (HandSign)aiSign,
                equippedSkin = SkinType.NeutralGloved,
                isPlayerA = false,
                isReady = true,
            },
            playerBConnected = true,
        };
        gameData.playerBJson = JsonUtility.ToJson(gameData.playerB);

        await db.RootReference.Child("games").Child(gameData.gameID).Child("playerB").SetValueAsync(gameData.playerBJson).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);
        });
        SceneController.Instance.GoToScene("RPSScene");
    }


    public async void CancelMatchMaking()
    {
        await RemoveWaitingGame();
        MainMenuManager.Instance.ToggleWaitingPanel();
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
