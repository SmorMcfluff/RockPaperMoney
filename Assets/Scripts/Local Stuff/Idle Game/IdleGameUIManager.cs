using TMPro;
using UnityEngine;

public class IdleGameUIManager : MonoBehaviour
{
    public static IdleGameUIManager Instance;

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI factoryText;
    [SerializeField] private TextMeshProUGUI priceText;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdateAllText()
    {
        UpdateMoneyText();
        UpdateFactoryText();
        UpdatePriceText(FactoryStore.Instance.CheckIfAffordable());
    }


    public void UpdateMoneyText()
    {
        moneyText.text = "Balance: $" + SaveDataManager.Instance.localPlayerData.moneyBalance;
    }


    public void UpdateFactoryText()
    {
        factoryText.text = "Factories: " + IdleGameManager.Instance.factories.Count;
    }

    public void UpdatePriceText(bool canAfford)
    {
        priceText.text = "$" + FactoryStore.Instance.factoryPrice.ToString("F2");

        if(canAfford)
        {
            priceText.color = Color.green;
        }
        else
        {
            priceText.color = Color.red;
        }
    }
}
