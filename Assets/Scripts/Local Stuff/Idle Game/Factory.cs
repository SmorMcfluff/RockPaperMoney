using System;
using UnityEngine;


public class Factory : MonoBehaviour
{
    [HideInInspector] public float moneyToVend = 0.01f;
    [HideInInspector] public float vendFrequency = 30;

    [HideInInspector] public float totalMoneyVended;
    [HideInInspector] public int totalVends;

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
        int vendsQueued = CheckVendsQueued(timeSinceVend);
        float vendAmount = moneyToVend * vendsQueued;

        SetTimeStamps();

        SaveDataManager.instance.localPlayerData.ChangeMoneyBalance(vendAmount);
        Debug.Log(SaveDataManager.instance.localPlayerData.moneyBalance.ToString());

        SaveDataManager.SavePlayer();
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