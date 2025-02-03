using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IdleGameUIManager : MonoBehaviour
{
    public static IdleGameUIManager Instance;

    [SerializeField] private GameObject factoryUIPrefab;
    private List<GameObject> factoryUIObjects = new();

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI factoryText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI efficiencyText;


    [SerializeField] private GameObject factoryView;
    [SerializeField] private RectTransform factoryViewContent;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            UpdateAllText();
        }
    }

    public void UpdateAllText()
    {
        UpdateMoneyText();
        UpdateFactoryText();
        UpdatePriceText(FactoryStore.Instance.CheckIfAffordable());
        UpdateEfficiencyText();
    }


    public void UpdateMoneyText()
    {
        moneyText.text = "Balance: $" + SaveDataManager.Instance.localPlayerData.moneyBalance;
        UpdatePriceText(FactoryStore.Instance.CheckIfAffordable());
    }


    public void UpdateFactoryText()
    {
        factoryText.text = "Factories: " + IdleGameManager.Instance.factories.Count;
    }


    public void UpdatePriceText(bool canAfford)
    {
        priceText.text = "$" + FactoryStore.Instance.factoryPrice.ToString("F2");

        if (canAfford)
        {
            priceText.color = Color.green;
        }
        else
        {
            priceText.color = Color.red;
        }
    }


    public void UpdateEfficiencyText()
    {
        efficiencyText.text = $"${IdleGameManager.Instance.moneyPerMinute:F2}/min";
    }

    
    public void GenerateFactoryUIList()
    {
        GenerateFactoryUIs();
        SetFactoryUIListSize();
    }

    public void ClearFactoryUIList()
    {
        foreach (GameObject go in factoryUIObjects)
        {
            Destroy(go);
        }

        factoryUIObjects.Clear();
    }


    private void GenerateFactoryUIs()
    {
        int factoryCount = IdleGameManager.Instance.factories.Count;
        for (int i = 0; i < factoryCount; i++)
        {
            GameObject newFactoryUIObject = Instantiate(factoryUIPrefab, factoryViewContent);
            newFactoryUIObject.GetComponent<FactoryUI>().Construct(i);

            factoryUIObjects.Add(newFactoryUIObject);
        }
    }


    private void SetFactoryUIListSize()
    {
        int factoryCount = IdleGameManager.Instance.factories.Count;

        float width = factoryViewContent.sizeDelta.x;
        float height = 128 + (factoryCount * 320);
        factoryViewContent.sizeDelta = new(width, height);
    }


    public void SetFactoryView(bool status)
    {
        if (status)
        {
            GenerateFactoryUIList();
        }
        else
        {
            ClearFactoryUIList();
        }

        factoryView.SetActive(status);
    }
}
