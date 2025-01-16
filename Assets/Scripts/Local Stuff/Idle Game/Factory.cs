using System;
using UnityEngine;


public class Factory
{
    [HideInInspector] public float moneyToVend = 0.01f;
    [HideInInspector] public float vendFrequency = 3;//weiner was hr3

    [HideInInspector] public int vendUpgrades = 0;
    [HideInInspector] public int frequencyUpgrades = 0;

    [HideInInspector] public DateTime lastVendTime;

    public Factory()
    {
        moneyToVend = 0.01f;
        vendFrequency = 3;//weiner was hr3

        vendUpgrades = 0;
        frequencyUpgrades = 0;
    }

    public Factory(FactoryData factoryData)
    {
        SetTimeStamps();

        moneyToVend = factoryData.moneyToVend;
        vendFrequency = factoryData.vendFrequency;

        lastVendTime = DateTime.Parse(factoryData.lastVendTimeString);
    }


    public void CheckTimeStamps()
    {
        var currentTime = DateTime.UtcNow;
        TimeSpan timeSinceVend = currentTime - lastVendTime;

        if (timeSinceVend.TotalSeconds > vendFrequency)
        {
            Vend(timeSinceVend);
        }
    }


    private void Vend(TimeSpan timeSinceVend)
    {
        float upgradedMoneyToVend = moneyToVend * Mathf.Pow(2, vendUpgrades);
        int vendsQueued = CheckVendsQueued(timeSinceVend);
        float vendAmount = upgradedMoneyToVend * vendsQueued;

        SetTimeStamps();

        SaveDataManager.Instance.localPlayerData.ChangeMoneyBalance(vendAmount);
        IdleGameUIManager.Instance.UpdateMoneyText();
    }


    private int CheckVendsQueued(TimeSpan timeSinceVend)
    {
        var unroundedVendsQueued = timeSinceVend.TotalSeconds / vendFrequency;
        int vendsQueued = (int)Math.Floor(unroundedVendsQueued);
        return vendsQueued;
    }


    public void SetTimeStamps()
    {
        lastVendTime = DateTime.UtcNow;
    }


    public void UpgradeVending()
    {
        if (vendUpgrades < 10)
        {
            vendUpgrades++;
            SaveDataManager.Instance.SavePlayer();
        }
    }


    public void UpgradeSpeed()
    {
        if (frequencyUpgrades < 25)
        {
            frequencyUpgrades++;
            SaveDataManager.Instance.SavePlayer();
        }
    }


    public FactoryData GetData()
    {
        var factoryData = new FactoryData();
        factoryData.moneyToVend = moneyToVend;
        factoryData.vendFrequency = vendFrequency;

        factoryData.lastVendTimeString = lastVendTime.ToString();

        return factoryData;
    }
}