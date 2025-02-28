using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryUI : MonoBehaviour
{
    private enum UpgradeType { money, frequency }

    private int factoryIndex;

    private float vendAmount;
    private float vendFrequency;

    [SerializeField] private GameObject IDCardPrefab;

    [SerializeField] private TextMeshProUGUI factoryNameText;

    [SerializeField] private TextMeshProUGUI factoryStatsText;

    [SerializeField] private TextMeshProUGUI moneyUpgradePriceText;
    [SerializeField] private TextMeshProUGUI frequencyUpgradePriceText;

    [SerializeField] private Button moneyUpgradeButton;
    [SerializeField] private Button frequencyUpgradeButton;


    public void Construct(int i)
    {
        factoryIndex = i;

        Factory parentFactory = IdleGameManager.Instance.factories[i];

        vendAmount = parentFactory.GetUpgradedMoneyToVend();
        vendFrequency = parentFactory.GetUpgradedVendFrequency();

        moneyUpgradeButton.onClick.AddListener(() => UpgradeFactory(UpgradeType.money));
        frequencyUpgradeButton.onClick.AddListener(() => UpgradeFactory(UpgradeType.frequency));

        UpdateText();
    }


    public void UpdateText()
    {
        Factory parentFactory = IdleGameManager.Instance.factories[factoryIndex];

        vendAmount = parentFactory.GetUpgradedMoneyToVend();
        vendFrequency = parentFactory.GetUpgradedVendFrequency();

        factoryNameText.text = $"{parentFactory.adWatcherInfo.firstName} {parentFactory.adWatcherInfo.lastName}";
        factoryStatsText.text = $"${vendAmount:N2}/{vendFrequency}s";

        if (parentFactory.moneyUpgrades < 10)
        {
            moneyUpgradePriceText.text = $"${parentFactory.moneyUpgradePrice:N2}";
        }
        else
        {
            moneyUpgradePriceText.text = "--";
        }

        if (parentFactory.frequencyUpgrades < 14)
        {
            frequencyUpgradePriceText.text = $"${parentFactory.frequencyUpgradePrice:N2}";
        }
        else
        {
            frequencyUpgradePriceText.text = "--";
        }
    }


    private void UpgradeFactory(UpgradeType upgradeType)
    {
        Factory parentFactory = IdleGameManager.Instance.factories[factoryIndex];

        if (upgradeType == UpgradeType.frequency)
        {
            parentFactory.UpgradeFrequency();
        }
        else if (upgradeType == UpgradeType.money)
        {
            parentFactory.UpgradeMoneyVendAmount();
        }

        UpdateText();
    }

    public void ShowWorkerIDCard()
    {
        var adInfo = IdleGameManager.Instance.factories[factoryIndex].adWatcherInfo;
        IDCardContainer.Instance.IDCard.LoadData(adInfo);
        IDCardContainer.Instance.IDCard.gameObject.SetActive(true);
    }
}
