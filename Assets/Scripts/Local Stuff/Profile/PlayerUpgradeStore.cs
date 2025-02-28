using TMPro;
using UnityEngine;

public class PlayerUpgradeStore : MonoBehaviour
{
    private const float moneyBasePrice = 5;
    private const float moneyMultiplier = 2;

    private const float timeBasePrice = 10;
    private const float timeMultiplier = 1.75f;

    private float moneyUpgradePrice;
    private float timeUpgradePrice;

    [SerializeField] private TextMeshProUGUI moneyStatusText;
    [SerializeField] private TextMeshProUGUI timeStatusText;

    [SerializeField] private TextMeshProUGUI moneyPriceText;
    [SerializeField] private TextMeshProUGUI timePriceText;

    private void Awake()
    {
        SetPricesOnAwake();
    }


    public void BuyMoneyUpgrade()
    {
        PlayerData localPlayer = SaveDataManager.Instance.localPlayerData;
        if (localPlayer.moneyUpgrades >= 10)
        {
            moneyPriceText.text = "--";
        }

        if (!CheckIfAffordable(localPlayer.GetMoneyBalance(), moneyUpgradePrice))
        {
            return;
        }

        localPlayer.moneyUpgrades++;
        localPlayer.ChangeMoneyBalance(-moneyUpgradePrice);
        ProfileSceneManager.Instance.UpdateBalanceText();

        moneyUpgradePrice = GetUpgradePrice(moneyBasePrice, moneyMultiplier, localPlayer.moneyUpgrades, moneyPriceText, moneyStatusText);
        InternetChecker.IsInternetAvailable(() => SaveDataManager.Instance.SavePlayer());
    }


    public void BuyTimeUpgrade()
    {
        PlayerData localPlayer = SaveDataManager.Instance.localPlayerData;
        if (localPlayer.timeUpgrades >= 14)
        {
            timePriceText.text = "--";
            return;
        }

        if (!CheckIfAffordable(localPlayer.GetMoneyBalance(), timeUpgradePrice))
        {
            return;
        }

        localPlayer.timeUpgrades++;
        localPlayer.ChangeMoneyBalance(-timeUpgradePrice);
        ProfileSceneManager.Instance.UpdateBalanceText();

        timeUpgradePrice = GetUpgradePrice(timeBasePrice, timeMultiplier, localPlayer.timeUpgrades, timePriceText, timeStatusText);
        InternetChecker.IsInternetAvailable(() => SaveDataManager.Instance.SavePlayer());
    }


    private float GetUpgradePrice(float basePrice, float multiplier, float upgradeCount, TextMeshProUGUI priceText, TextMeshProUGUI statusText)
    {
        float newPrice = Mathf.Round(basePrice * Mathf.Pow(multiplier, upgradeCount) * 100f) / 100f;
        SetText(newPrice, priceText, statusText);
        return newPrice;
    }


    private void SetPricesOnAwake()
    {
        PlayerData localPlayer = SaveDataManager.Instance.localPlayerData;
        if (localPlayer.moneyUpgrades < 10)
        {
            moneyUpgradePrice = GetUpgradePrice(moneyBasePrice, moneyMultiplier, localPlayer.moneyUpgrades, moneyPriceText, moneyStatusText);

        }
        else
        {
            moneyPriceText.text = "--";
        }

        if (localPlayer.timeUpgrades < 14)
        {
            timeUpgradePrice = GetUpgradePrice(timeBasePrice, timeMultiplier, localPlayer.timeUpgrades, timePriceText, timeStatusText);
        }
        else
        {
            timePriceText.text = "--";
        }
    }


    private void SetText(float price, TextMeshProUGUI priceText, TextMeshProUGUI statusText)
    {
        priceText.text = $"${price:N0}";

        statusText.text = (statusText == moneyStatusText)
            ? $"${AdPlayer.GetAdMoneyAmount()} per watched ad"
            : $"Minimum ad time: {AdPlayer.GetMinViewTime()}s";
    }

    private bool CheckIfAffordable(float balance, float price)
    {
        return balance >= price;
    }
}
