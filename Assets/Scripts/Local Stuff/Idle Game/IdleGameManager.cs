using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleGameManager : MonoBehaviour
{
    public static IdleGameManager Instance;

    public List<Factory> factories = new List<Factory>();

    [DoNotSerialize] public float moneyPerMinute;

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

    private void Start()
    {
        SaveDataManager.Instance.InvokeRepeating(nameof(SaveDataManager.Instance.SavePlayer), 3, 30);
    }


    private void Update()
    {
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

        GetMoneyPerMinute();
        FactoryStore.Instance.SetFactoryPrice();
        IdleGameUIManager.Instance.UpdateAllText();

        SaveDataManager.Instance.localPlayerData.factoriesJsonStrings.Add(newFactoryJsonString);
        SaveDataManager.Instance.SavePlayer();

    }


    public void LoadFactory(int i)
    {
        Debug.Log($"loading factory index {i}");

        var newFactoryJsonString = SaveDataManager.Instance.localPlayerData.factoriesJsonStrings[i];
        FactoryData newFactoryData = JsonUtility.FromJson<FactoryData>(newFactoryJsonString);

        var newFactory = new Factory(newFactoryData);
        factories.Add(newFactory);

        GetMoneyPerMinute();
    }


    public void GetMoneyPerMinute()
    {
        float productionPerSecond = 0;
        foreach (var factory in factories)
        {
            float factoryProduction = factory.GetUpgradedMoneyToVend();
            float factoryFrequency = factory.GetUpgradedVendFrequency();

            float factoryEfficiency = (factoryProduction / factoryFrequency);
            productionPerSecond += factoryEfficiency;
        }

        moneyPerMinute = productionPerSecond * 60;
    }
}
