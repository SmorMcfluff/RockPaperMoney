using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager instance;
    public PlayerData localPlayerData;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("instance of" + GetType() + " already exists");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        LoadPlayer();
    }


    public static void LoadPlayer()
    {
        instance.localPlayerData = new PlayerData();


        if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerSaveData")))
        {
            SavePlayer(); //Make an empty save state - a new player
        }

        var loadedJson = PlayerPrefs.GetString("PlayerSaveData");

        var saveData = JsonUtility.FromJson<PlayerData>(loadedJson);
        instance.localPlayerData = saveData;

        Debug.Log(JsonUtility.ToJson(instance.localPlayerData));

        if (instance.localPlayerData.factoriesJsonStrings.Count > 0)
        {
            for (int i = 0; i < instance.localPlayerData.factoriesJsonStrings.Count; i++)
            {
                IdleGameManager.instance.LoadFactory(i);
            }
        }
    }


    public static void SavePlayer()
    {

        for (int i = 0; i < instance.localPlayerData.factoriesJsonStrings.Count; i++)
        {
            SaveFactory(i);
        }

        PlayerData saveData = instance.localPlayerData;

        string jsonString = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString("PlayerSaveData", jsonString);
    }

    private static void SaveFactory(int i)
    {
        var factoryData = IdleGameManager.instance.factories[i].GetData();
        instance.localPlayerData.factoriesJsonStrings[i] = JsonUtility.ToJson(factoryData);
    }
}
