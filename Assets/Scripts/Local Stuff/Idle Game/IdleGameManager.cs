using System.Collections.Generic;
using UnityEngine;

public class IdleGameManager : MonoBehaviour
{
    public static IdleGameManager Instance;

    public List<Factory> factories = new List<Factory>();

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
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddFactory();
        }

        HandleFactories();
    }


    private void HandleFactories()
    {
        foreach (Factory factory in factories)
        {
            factory.CheckTimeStamps();
        }
    }


    public void AddFactory()
    {
        var newFactory = new Factory();
        newFactory.SetTimeStamps();

        var newFactoryJsonString = JsonUtility.ToJson(newFactory);

        factories.Add(newFactory);

        IdleGameUIManager.Instance.UpdateAllText();
        FactoryStore.Instance.SetFactoryPrice();

        SaveDataManager.Instance.localPlayerData.factoriesJsonStrings.Add(newFactoryJsonString);
        SaveDataManager.Instance.SavePlayer();

    }


    public void LoadFactory(int i)
    {
        var newFactoryJsonString = SaveDataManager.Instance.localPlayerData.factoriesJsonStrings[i];
        FactoryData newFactoryData = JsonUtility.FromJson<FactoryData>(newFactoryJsonString);

        Debug.Log("Loading factory: " + newFactoryJsonString);
        var newFactory = new Factory(newFactoryData);
        factories.Add(newFactory);

        FactoryStore.Instance.SetFactoryPrice();
        IdleGameUIManager.Instance.UpdateAllText();
    }
}
