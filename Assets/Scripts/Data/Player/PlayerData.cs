using System.Collections.Generic;
using System.Globalization;

public class PlayerData
{
    public string username;

    public int winCount;
    public int lossCount;

    public bool rockUnlocked = true;
    public bool paperUnlocked = true;
    public bool scissorsUnlocked = true;

    public string moneyBalance = "0.01";

    public List<string> factoriesJsonStrings = new();

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
}