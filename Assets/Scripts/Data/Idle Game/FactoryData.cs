using System;
using UnityEngine;

public class FactoryData
{
    public float moneyToVend;
    public float vendFrequency;

    public float totalMoneyVended;
    public int totalVends;

    public int vendUpgrades = 0;
    public int speedUpgrades = 0;

    public DateTime lastVendTime;
    public string lastVendTimeString;
}
