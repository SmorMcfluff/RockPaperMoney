using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

public class PlayerData
{
    public string username;

    public int winCount;
    public int lossCount;

    public bool[] unlockedHandSigns = new bool[3];
    public bool hasAdam = false;

    public List<SkinType> ownedSkins = new()
    {
        SkinType.Default
    };

    public SkinType equippedSkin = SkinType.Default;

    public string moneyBalance = "0.01";

    public List<FactoryData> factories = new();

    public void ChangeMoneyBalance(float amount)
    {
        float moneyAmount = float.Parse(moneyBalance, CultureInfo.InvariantCulture);
        moneyAmount += amount;
        moneyBalance = moneyAmount.ToString("F2", CultureInfo.InvariantCulture);
    }


    public float GetMoneyBalance()
    {
        return float.Parse(moneyBalance, CultureInfo.InvariantCulture);
    }


    public void SetInitialUnlockedHandsigns()
    {
        unlockedHandSigns[Random.Range(0, 3)] = true;
    }

    public PlayerData()
    {
        SetInitialUnlockedHandsigns();
    }
}