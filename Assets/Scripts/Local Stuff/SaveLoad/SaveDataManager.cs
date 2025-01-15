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
        }

        DontDestroyOnLoad(gameObject);

        LoadPlayer();
    }


    public static void LoadPlayer()
    {
        instance.localPlayerData = new PlayerData();

        var loadedJson = PlayerPrefs.GetString("PlayerSaveData");

        if (string.IsNullOrEmpty(loadedJson)) //Make an empty save state - a new player
        {
            SavePlayer();
            loadedJson = PlayerPrefs.GetString("PlayerSaveData");
        }

        var saveData = JsonUtility.FromJson<PlayerData>(loadedJson);

        instance.localPlayerData = saveData;

        Debug.Log(JsonUtility.ToJson(instance.localPlayerData));

        for(int i = 0; i < instance.localPlayerData.factories.Count; i++)
        {
            IdleGameManager.instance.LoadFactory(i);
        }
    }

    public static void SavePlayer()
    {
        PlayerData saveData = instance.localPlayerData;

        string jsonString = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString("PlayerSaveData", jsonString);
    }
}
