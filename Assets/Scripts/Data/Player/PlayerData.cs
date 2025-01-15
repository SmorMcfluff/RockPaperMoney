using System.Collections.Generic;

public class PlayerData
{
    public string username = "";

    public int winCount = 0;
    public int lossCount = 0;

    public bool rockUnlocked = true;
    public bool paperUnlocked = true;
    public bool scissorsUnlocked = true;

    public string moneyBalance = "0";

    public List<string> factoriesJsonStrings = new();

    public void ChangeMoneyBalance(float amount)
    {
        float moneyAmount = float.Parse(moneyBalance);
        moneyAmount += amount;
        moneyBalance = moneyAmount.ToString("F2");
    }
}