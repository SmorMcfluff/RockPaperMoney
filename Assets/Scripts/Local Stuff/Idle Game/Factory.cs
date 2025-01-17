using System;
using UnityEngine;


public class Factory
{
    [HideInInspector] public float baseMoneyToVend = 0.01f;

    [HideInInspector] public float baseVendFrequency = 5;//weiner was hr3
    [HideInInspector] private float upgradedVendFrequency;

    [HideInInspector] public int moneyUpgrades;
    [HideInInspector] public int frequencyUpgrades;

    private float baseMoneyUpgradePrice = 0.5f;
    private float baseFrequencyUpgradePrice = 1.0f;

    [HideInInspector] public float moneyUpgradePrice;
    [HideInInspector] public float frequencyUpgradePrice;

    [HideInInspector] public DateTime lastVendTime;

    public Factory()
    {
        baseVendFrequency = 5;//weiner was hr3
        upgradedVendFrequency = GetUpgradedVendFrequency();

        moneyUpgrades = 0;
        frequencyUpgrades = 0;
        
        SetMoneyUpgradePrice();
        SetFrequencyUpgradePrice();
    }

    public Factory(FactoryData factoryData)
    {
        SetTimeStamps();

        baseMoneyToVend = factoryData.moneyToVend;
        baseVendFrequency = factoryData.baseVendFrequency;


        moneyUpgrades = factoryData.moneyUpgrades;
        frequencyUpgrades = factoryData.frequencyUpgrades;

        lastVendTime = DateTime.Parse(factoryData.lastVendTimeString);

        SetMoneyUpgradePrice();
        SetFrequencyUpgradePrice();
    }


    public void CheckTimeStamps()
    {
        var currentTime = DateTime.UtcNow;
        TimeSpan timeSinceVend = currentTime - lastVendTime;

        if (timeSinceVend.TotalSeconds > upgradedVendFrequency)
        {
            Vend(timeSinceVend);
        }
    }


    private void Vend(TimeSpan timeSinceVend)
    {
        float upgradedMoneyToVend = GetUpgradedMoneyToVend();
        int vendsQueued = CheckVendsQueued(timeSinceVend);
        float vendAmount = upgradedMoneyToVend * vendsQueued;

        SetTimeStamps();

        SaveDataManager.Instance.localPlayerData.ChangeMoneyBalance(vendAmount);
        IdleGameUIManager.Instance.UpdateMoneyText();
    }

    public float GetUpgradedMoneyToVend()
    {
        return baseMoneyToVend * Mathf.Pow(2, moneyUpgrades);
    }

    public float GetUpgradedVendFrequency()
    {
        return MathF.Floor(baseVendFrequency - 1 * frequencyUpgrades);
    }


    private int CheckVendsQueued(TimeSpan timeSinceVend)
    {
        var unroundedVendsQueued = timeSinceVend.TotalSeconds / baseVendFrequency;
        int vendsQueued = (int)Math.Floor(unroundedVendsQueued);
        return vendsQueued;
    }


    public void SetTimeStamps()
    {
        lastVendTime = DateTime.UtcNow;
    }


    public void UpgradeMoneyVendAmount()
    {
        if (moneyUpgrades < 10 && float.Parse(SaveDataManager.Instance.localPlayerData.moneyBalance) >= moneyUpgradePrice)
        {
            moneyUpgrades++;
            SaveDataManager.Instance.localPlayerData.ChangeMoneyBalance(-moneyUpgradePrice);
            
            SetMoneyUpgradePrice();

            SaveDataManager.Instance.SavePlayer();
        }
    }


    public void UpgradeFrequency()
    {
        if (frequencyUpgrades < 25 && float.Parse(SaveDataManager.Instance.localPlayerData.moneyBalance) >= frequencyUpgradePrice)
        {
            frequencyUpgrades++;
            upgradedVendFrequency = GetUpgradedVendFrequency();
            SaveDataManager.Instance.localPlayerData.ChangeMoneyBalance(-frequencyUpgradePrice);

            SetFrequencyUpgradePrice();

            SaveDataManager.Instance.SavePlayer();
        }
    }


    public FactoryData GetData()
    {
        var factoryData = new FactoryData();
        factoryData.moneyToVend = baseMoneyToVend;
        factoryData.baseVendFrequency = baseVendFrequency;

        factoryData.lastVendTimeString = lastVendTime.ToString();

        return factoryData;
    }


    public void SetMoneyUpgradePrice()
    {
        moneyUpgradePrice = baseMoneyUpgradePrice * Mathf.Pow(2, moneyUpgrades);
    }

    public void SetFrequencyUpgradePrice()
    {
        frequencyUpgradePrice = Mathf.Floor(baseFrequencyUpgradePrice * Mathf.Pow(1.75f, frequencyUpgrades) * 100) / 100;
    }
}