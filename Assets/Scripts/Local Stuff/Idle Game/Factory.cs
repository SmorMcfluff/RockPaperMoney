using System;
using UnityEngine;


public class Factory : MonoBehaviour
{
    public decimal moneyToVend = 0.01m;
    public float vendFrequency = 3;

    public float totalMoneyVended;
    public int totalVends;

    public DateTime lastVendTime;

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
            SetTimeStamps();
        }
    }


    private void Vend(TimeSpan timeSinceVend)
    {
        int vendsQueued = CheckVendsQueued(timeSinceVend);
        decimal vendedAmount = moneyToVend * vendsQueued;

        Debug.Log(vendsQueued + " vends for a total of " + vendedAmount);

        SaveDataManager.instance.localPlayerData.ChangeMoneyBalance(vendedAmount);

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


    public void LoadData(Factory factoryData)
    {
        moneyToVend = factoryData.moneyToVend;
        vendFrequency = factoryData.vendFrequency;
        totalMoneyVended = factoryData.totalMoneyVended;
        totalVends = factoryData.totalVends;
        lastVendTime = factoryData.lastVendTime;
    }
}
