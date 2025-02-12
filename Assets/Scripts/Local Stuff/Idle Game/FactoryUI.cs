using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryUI : MonoBehaviour
{
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

    private enum UpgradeType { money, frequency }

    public void Construct(int i)
    {
        factoryIndex = i;

        Factory parentFactory = IdleGameManager.Instance.factories[i];

        vendAmount = parentFactory.GetUpgradedMoneyToVend();
        vendFrequency = parentFactory.GetUpgradedVendFrequency();

        moneyUpgradeButton.onClick.AddListener(delegate { UpgradeFactory(UpgradeType.money); });
        frequencyUpgradeButton.onClick.AddListener(delegate { UpgradeFactory(UpgradeType.frequency); });

        UpdateText();
    }


    public void UpdateText()
    {
        Factory parentFactory = IdleGameManager.Instance.factories[factoryIndex];

        vendAmount = parentFactory.GetUpgradedMoneyToVend();
        vendFrequency = parentFactory.GetUpgradedVendFrequency();

        factoryNameText.text = $"{parentFactory.adWatcherInfo.firstName} {parentFactory.adWatcherInfo.lastName}";
        factoryStatsText.text = $"${vendAmount:F2}/{vendFrequency}s";

        if (parentFactory.moneyUpgrades < 10)
        {
            moneyUpgradePriceText.text = $"${parentFactory.moneyUpgradePrice:F2}";
        }
        else
        {
            moneyUpgradePriceText.text = "--";
        }

        if (parentFactory.frequencyUpgrades < 14)
        {
            frequencyUpgradePriceText.text = $"${parentFactory.frequencyUpgradePrice:F2}";
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
