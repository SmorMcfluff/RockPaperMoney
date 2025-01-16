using UnityEngine;

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
            DontDestroyOnLoad(gameObject);
        }
        LoadPlayer();
    }

    private void Start()
    {
        IdleGameUIManager.Instance.UpdateAllText();
        Instance.InvokeRepeating(nameof(SavePlayer), 3, 30);
    }


    public static void LoadPlayer()
    {
        Instance.localPlayerData = new PlayerData();


        if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerSaveData")))
        {
            Instance.SavePlayer(); //Make an empty save state - a new player
        }

        var loadedJson = PlayerPrefs.GetString("PlayerSaveData");

        var saveData = JsonUtility.FromJson<PlayerData>(loadedJson);
        Instance.localPlayerData = saveData;

        Debug.Log(JsonUtility.ToJson(Instance.localPlayerData));

        if (Instance.localPlayerData.factoriesJsonStrings.Count > 0)
        {
            for (int i = 0; i < Instance.localPlayerData.factoriesJsonStrings.Count; i++)
            {
                IdleGameManager.Instance.LoadFactory(i);
            }
        }
    }


    public void SavePlayer()
    {
        for (int i = 0; i < Instance.localPlayerData.factoriesJsonStrings.Count; i++)
        {
            SaveFactory(i);
        }

        PlayerData saveData = Instance.localPlayerData;

        string jsonString = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString("PlayerSaveData", jsonString);
    }


    private static void SaveFactory(int i)
    {
        var factoryData = IdleGameManager.Instance.factories[i].GetData();
        Instance.localPlayerData.factoriesJsonStrings[i] = JsonUtility.ToJson(factoryData);
    }


    private void OnApplicationQuit()
    {
        SavePlayer();
    }


    private void OnApplicationFocus(bool focus)
    {
        SavePlayer();
    }
}
