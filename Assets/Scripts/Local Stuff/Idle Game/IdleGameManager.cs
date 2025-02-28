using System.Collections.Generic;
using UnityEngine;

public class IdleGameManager : MonoBehaviour
{
    public static IdleGameManager Instance;

    public List<Factory> factories = new List<Factory>();

    [HideInInspector] public float moneyPerMinute;

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

        factories.Add(newFactory);

        GetMoneyPerMinute();
        FactoryStore.Instance.SetFactoryPrice();
        IdleGameUIManager.Instance.UpdateAllText();

        SaveDataManager.Instance.localPlayerData.factories.Add(newFactory.GetData());
        InternetChecker.IsInternetAvailable(() => SaveDataManager.Instance.SavePlayer());

    }


    public void LoadFactory(int i)
    {
        FactoryData newFactoryData = SaveDataManager.Instance.localPlayerData.factories[i];

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
