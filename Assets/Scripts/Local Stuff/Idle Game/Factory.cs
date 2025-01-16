using System;
using UnityEngine;


public class Factory : MonoBehaviour
{
    [HideInInspector] public float moneyToVend = 0.01f;
    [HideInInspector] public float vendFrequency = 3;//weiner was h3r3

    [HideInInspector] public float totalMoneyVended;
    [HideInInspector] public int totalVends;

    [HideInInspector] public int vendUpgrades = 0;
    [HideInInspector] public int frequencyUpgrades = 0;

    [HideInInspector] public DateTime lastVendTime;

    private void Awake()
    {
        SetTimeStamps();
    }


    private void Update()
    {
        CheckTimeStamps();
    }


    private void CheckTimeStamps()
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


    private void SetTimeStamps()
    {
        lastVendTime = DateTime.UtcNow;
    }


    public void UpgradeVending()
    {
        if(vendUpgrades < 10)
        { 
            vendUpgrades++;
            SaveDataManager.Instance.SavePlayer();
        }
    }


    public void UpgradeSpeed()
    {
        if(frequencyUpgrades < 25)
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

        factoryData.totalMoneyVended = totalMoneyVended;
        factoryData.totalVends = totalVends;

        factoryData.lastVendTimeString = lastVendTime.ToString();

        return factoryData;
    }


    public void SetData(FactoryData factoryData)
    {
        moneyToVend = factoryData.moneyToVend;
        vendFrequency = factoryData.vendFrequency;

        totalMoneyVended = factoryData.totalMoneyVended;
        totalVends = factoryData.totalVends;

        lastVendTime = DateTime.Parse(factoryData.lastVendTimeString);
    }
}