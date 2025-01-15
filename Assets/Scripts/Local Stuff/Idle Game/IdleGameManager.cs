using System.Collections.Generic;
using UnityEngine;

public class IdleGameManager : MonoBehaviour
{
    public static IdleGameManager instance;

    public List<Factory> factories;
    public GameObject factoryPrefab;

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
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddFactory();
        }
    }


    public void AddFactory()
    {
        var newFactoryObject = Instantiate(factoryPrefab);
        var newFactory = newFactoryObject.GetComponent<Factory>();

        var newFactoryJsonString = JsonUtility.ToJson(newFactory);

        factories.Add(newFactory);
        SaveDataManager.instance.localPlayerData.factoriesJsonStrings.Add(newFactoryJsonString);
        SaveDataManager.SavePlayer();
    }


    public void LoadFactory(int i)
    {
        var newFactoryJsonString = SaveDataManager.instance.localPlayerData.factoriesJsonStrings[i];
        FactoryData newFactoryData = JsonUtility.FromJson<FactoryData>(newFactoryJsonString);

        var newFactoryObject = Instantiate(factoryPrefab);
        newFactoryObject.GetComponent<Factory>().SetData(newFactoryData);
        factories.Add(newFactoryObject.GetComponent<Factory>());
    }
}
