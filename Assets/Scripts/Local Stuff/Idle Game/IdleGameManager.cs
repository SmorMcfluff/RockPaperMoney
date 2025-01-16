using System.Collections.Generic;
using UnityEngine;

public class IdleGameManager : MonoBehaviour
{
    public static IdleGameManager Instance;

    public List<Factory> factories;
    public GameObject factoryPrefab;

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

        SaveDataManager.Instance.localPlayerData.factoriesJsonStrings.Add(newFactoryJsonString);
        IdleGameUIManager.Instance.UpdateFactoryText();
        FactoryStore.Instance.SetFactoryPrice();
        SaveDataManager.Instance.SavePlayer();
    }


    public void LoadFactory(int i)
    {
        var newFactoryJsonString = SaveDataManager.Instance.localPlayerData.factoriesJsonStrings[i];
        FactoryData newFactoryData = JsonUtility.FromJson<FactoryData>(newFactoryJsonString);

        var newFactoryObject = Instantiate(factoryPrefab);
        newFactoryObject.GetComponent<Factory>().SetData(newFactoryData);
        factories.Add(newFactoryObject.GetComponent<Factory>());
    }
}
