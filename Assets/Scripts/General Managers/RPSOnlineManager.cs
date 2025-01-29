/*
 * Special thanks to  Salmaster for letting
 * me take a look at his matchmaking code.
 * I couldn't have done it without you.
 */

using Firebase.Database;
using Firebase.Extensions;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RPSOnlineManager : MonoBehaviour
{
    public static RPSOnlineManager Instance;

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
        }

        DontDestroyOnLoad(gameObject);
        db = FirebaseDatabase.DefaultInstance;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            CreateGame();

        if (Input.GetKeyDown(KeyCode.V))
            ConnectToGame();
    }


    public void ConnectToGame()
    {
        Debug.Log("Trying to connect");
        LoadGames(onLoaded);
    }


    private void LoadGames(OnLoadedDelegate onLoadedDelegate)
    {
        Debug.Log("Loading Games");
        db.RootReference.Child("games").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            DataSnapshot snap = task.Result;

            CheckLoadedGames(snap);
        });
    }


    private void CheckLoadedGames(DataSnapshot snap)
    {
        var snapshots = snap.Children;
        Debug.Log(snapshots.Count());
        List<RPSGameData> rpsGames = new();
        Debug.Log("Checking loaded games");

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

                var rpsPlayer = new RPSPlayer(SaveDataManager.Instance.localPlayerData);
                localPlayer = rpsPlayer;
                rpsGame.playerBJson = JsonUtility.ToJson(rpsPlayer);
                rpsGame.playerBConnected = true;
                db.RootReference.Child("games").Child(rpsGame.gameID).SetValueAsync(JsonUtility.ToJson(rpsGame)).ContinueWithOnMainThread(task =>
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
            Debug.Log("YEAH");
            SceneController.Instance.GoToScene("RPSScene");
        }
        else
        {
            Debug.Log("NUHUH");
            CreateGame();
        }
    }


    public void CreateGame()
    {
        Debug.Log("Game Created");
        localPlayer = new RPSPlayer(SaveDataManager.Instance.localPlayerData)
        {
            isPlayerA = true
        };
        
        var rpsGame = new RPSGameData(localPlayer);
        var rpsGameJson = JsonUtility.ToJson(rpsGame);

        gameData = rpsGame;

        db.RootReference.Child("games").Child(gameData.gameID).SetValueAsync(rpsGameJson).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }

        });
        db.RootReference.Child("games").Child(gameData.gameID).ValueChanged += ValueChanged;
    }

    private void ValueChanged(object sender, ValueChangedEventArgs args)
    {
        db.RootReference.Child("games").Child(gameData.gameID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }

            var snap = task.Result;

            var snapJson = Regex.Unescape(snap.GetRawJsonValue()).Trim('"');
            var data = JsonUtility.FromJson<RPSGameData>(snapJson);

            if (data.playerAConnected && data.playerBConnected)
            {
                SceneController.Instance.GoToScene("RPSScene");
            }
        });

    }
}
