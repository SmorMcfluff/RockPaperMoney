using System;
using System.Globalization;
using UnityEngine;


public class Factory
{
    [HideInInspector] public AdWatcherInfo adWatcherInfo;

    [HideInInspector] public float baseMoneyToVend = 0.01f;

    [HideInInspector] public float baseVendFrequency = 15f;//weiner was hr3
    [HideInInspector] private float upgradedVendFrequency;

    [HideInInspector] public int moneyUpgrades;
    [HideInInspector] public int frequencyUpgrades;

    private float baseMoneyUpgradePrice = 0.5f;
    private float baseFrequencyUpgradePrice = 1.0f;

    [HideInInspector] public float moneyUpgradePrice;
    [HideInInspector] public float frequencyUpgradePrice;

    [HideInInspector] public DateTime lastVendTime;

    public Factory() //Bought factory
    {
        adWatcherInfo = new AdWatcherInfo();

        baseMoneyToVend = (adWatcherInfo.lastName == "Mcfluff")
            ? 0.02f
            : 0.01f;

        moneyUpgrades = 0;
        frequencyUpgrades = 0;

        upgradedVendFrequency = GetUpgradedVendFrequency();

        SetMoneyUpgradePrice();
        SetFrequencyUpgradePrice();
    }


    public Factory(FactoryData factoryData) //Loaded factory
    {
        SetTimeStamps();
        adWatcherInfo = factoryData.adWatcherInfo;

        baseMoneyToVend = factoryData.moneyToVend;
        baseVendFrequency = factoryData.baseVendFrequency;

        moneyUpgrades = factoryData.moneyUpgrades;
        frequencyUpgrades = factoryData.frequencyUpgrades;

        upgradedVendFrequency = GetUpgradedVendFrequency();

        lastVendTime = DateTime.Parse(factoryData.lastVendTimeString, CultureInfo.InvariantCulture);

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

        if (IdleGameUIManager.Instance != null)
        {
            IdleGameUIManager.Instance.UpdateBalanceText();
        }

        if (StoreManager.Instance != null)
        {
            StoreManager.Instance.UpdateBalanceText();
        }

        if(ProfileSceneManager.Instance != null)
        {
            ProfileSceneManager.Instance.UpdateBalanceText();
        }
    }


    public float GetUpgradedMoneyToVend()
    {
        return baseMoneyToVend * Mathf.Pow(2, moneyUpgrades);
    }


    public float GetUpgradedVendFrequency()
    {
        return MathF.Floor(baseVendFrequency - frequencyUpgrades);
    }


    private int CheckVendsQueued(TimeSpan timeSinceVend)
    {
        var unroundedVendsQueued = timeSinceVend.TotalSeconds / upgradedVendFrequency;
        int vendsQueued = (int)Math.Floor(unroundedVendsQueued);
        return vendsQueued;
    }


    public void SetTimeStamps()
    {
        lastVendTime = DateTime.UtcNow;
    }


    public void UpgradeMoneyVendAmount()
    {
        float moneyBalance = SaveDataManager.Instance.localPlayerData.GetMoneyBalance();
        if (moneyUpgrades < 10 && moneyBalance >= moneyUpgradePrice)
        {
            moneyUpgrades++;
            SaveDataManager.Instance.localPlayerData.ChangeMoneyBalance(-moneyUpgradePrice);

            SetMoneyUpgradePrice();
            IdleGameManager.Instance.GetMoneyPerMinute();

            try
            {
                IdleGameUIManager.Instance.UpdateBalanceText();
                IdleGameUIManager.Instance.UpdateEfficiencyText();
            }
            catch { }

            InternetChecker.IsInternetAvailable(() => SaveDataManager.Instance.SavePlayer());
        }
    }


    public void UpgradeFrequency()
    {
        float moneyBalance = SaveDataManager.Instance.localPlayerData.GetMoneyBalance();
        if (frequencyUpgrades < 14 && moneyBalance >= frequencyUpgradePrice)
        {
            frequencyUpgrades++;
            upgradedVendFrequency = GetUpgradedVendFrequency();
            SaveDataManager.Instance.localPlayerData.ChangeMoneyBalance(-frequencyUpgradePrice);
            IdleGameManager.Instance.GetMoneyPerMinute();

            try
            {
                IdleGameUIManager.Instance.UpdateBalanceText();
                IdleGameUIManager.Instance.UpdateEfficiencyText();
            }
            catch { }

            SetFrequencyUpgradePrice();

            InternetChecker.IsInternetAvailable(() => SaveDataManager.Instance.SavePlayer());
        }
    }


    public FactoryData GetData()
    {
        var factoryData = new FactoryData
        {
            adWatcherInfo = adWatcherInfo,
            moneyToVend = baseMoneyToVend,
            baseVendFrequency = baseVendFrequency,

            moneyUpgrades = moneyUpgrades,
            frequencyUpgrades = frequencyUpgrades,

            lastVendTimeString = lastVendTime.ToString()
        };

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