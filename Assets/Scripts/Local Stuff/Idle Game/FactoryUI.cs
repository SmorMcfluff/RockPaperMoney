using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryUI : MonoBehaviour
{
    private int factoryIndex;

    private float vendAmount;
    private float vendFrequency;

    private int moneyUpgrades;
    private int frequencyUpgrades;

    [SerializeField] private TextMeshProUGUI factoryStatsText;

    [SerializeField] private TextMeshProUGUI moneyUpgradePriceText;
    [SerializeField] private TextMeshProUGUI frequencyUpgradePriceText;

    [SerializeField] private Button moneyUpgradeButton;
    [SerializeField] private Button frequencyUpgradeButton;

    private enum UpgradeType {money, frequency}

    private UpgradeType upgradeType;


    public void Construct(int i)
    {
        factoryIndex = i;

        Factory parentFactory = IdleGameManager.Instance.factories[i];

        vendAmount = parentFactory.GetUpgradedMoneyToVend();
        vendFrequency = parentFactory.GetUpgradedVendFrequency();

        moneyUpgrades = parentFactory.moneyUpgrades;
        frequencyUpgrades = parentFactory.frequencyUpgrades;

        moneyUpgradeButton.onClick.AddListener(delegate { UpgradeFactory(UpgradeType.money); });
        frequencyUpgradeButton.onClick.AddListener(delegate { UpgradeFactory(UpgradeType.frequency); });

        UpdateText();
    }

    public void UpdateText()
    {
        Factory parentFactory = IdleGameManager.Instance.factories[factoryIndex];

        vendAmount = parentFactory.GetUpgradedMoneyToVend();
        vendFrequency = parentFactory.GetUpgradedVendFrequency();

        factoryStatsText.text = $"${vendAmount.ToString("F2")}/{vendFrequency}s";
        moneyUpgradePriceText.text = $"${parentFactory.moneyUpgradePrice.ToString("F2")}";
        frequencyUpgradePriceText.text = $"${parentFactory.frequencyUpgradePrice.ToString("F2")}";
    }

    private void UpgradeFactory(UpgradeType upgradeType)
    {
        Factory parentFactory = IdleGameManager.Instance.factories[factoryIndex];

        if (upgradeType == UpgradeType.frequency)
        {
            parentFactory.UpgradeFrequency();
        }
        else if(upgradeType == UpgradeType.money)
        {
            parentFactory.UpgradeMoneyVendAmount();
        }

        UpdateText();
    }
}
