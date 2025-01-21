using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager Instance;
    public PlayerData localPlayerData;

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
        localPlayerData = new PlayerData();
        DontDestroyOnLoad(gameObject);
    }


    public static object GetCurrentTimeStampForFirebaseServer()
    {
        return ServerValue.Timestamp;
    }


    public void LoadPlayer()
    {
        var db = FirebaseDatabase.DefaultInstance;
        var auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser == null)
        {
            Debug.LogError("No authenticated user found.");
            return;
        }

        var userID = auth.CurrentUser.UserId;

        db.RootReference.Child("users").Child(userID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Firebase Task Exception: " + task.Exception);
                return;
            }

            DataSnapshot snap = task.Result;
            if (snap == null || snap.GetRawJsonValue() == null)
            {
                Debug.LogWarning("Data snapshot is null or empty.");
                return;
            }

            string rawJson = snap.GetRawJsonValue();
            try
            {
                string cleanedJson = System.Text.RegularExpressions.Regex.Unescape(rawJson).Trim('"');
                localPlayerData = JsonUtility.FromJson<PlayerData>(cleanedJson);
            }
            catch (Exception ex)
            {
                Debug.LogError("Deserialization failed: " + ex.Message);
                return;
            }

            if (localPlayerData.factoriesJsonStrings != null && localPlayerData.factoriesJsonStrings.Count > 0)
            {
                for (int i = 0; i < localPlayerData.factoriesJsonStrings.Count; i++)
                {
                    IdleGameManager.Instance.LoadFactory(i);
                }
            }
        });

        if (IdleGameUIManager.Instance != null)
        {
            IdleGameUIManager.Instance.UpdateAllText();
        }
    }


    public void SavePlayer()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {

            var db = FirebaseDatabase.DefaultInstance;
            var auth = FirebaseAuth.DefaultInstance;

            if (localPlayerData == null)
            {
                localPlayerData = new PlayerData();
            }

            if (IdleGameManager.Instance != null)
            {
                int factoryCount = IdleGameManager.Instance.factories.Count;
                if (factoryCount > 0)
                {
                    for (int i = 0; i < factoryCount; i++)
                    {
                        SaveFactory(i);
                    }
                }
            }

            var userID = auth.CurrentUser.UserId;
            var data = JsonUtility.ToJson(localPlayerData);
            db.RootReference.Child("users").Child(userID).SetValueAsync(data).ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogWarning(task.Exception);
                }
            });
        }
    }


    private static void SaveFactory(int i)
    {
        var factoryData = IdleGameManager.Instance.factories[i].GetData();
        Instance.localPlayerData.factoriesJsonStrings[i] = JsonUtility.ToJson(factoryData);
    }


    private void OnApplicationQuit()
    {
        if (!SceneManager.GetActiveScene().name.Contains("Login"))
        {
            SavePlayer();
        }
    }


    private void OnApplicationFocus(bool focus)
    {
        if (!SceneManager.GetActiveScene().name.Contains("Login"))
        {
            SavePlayer();
        }
    }
}
