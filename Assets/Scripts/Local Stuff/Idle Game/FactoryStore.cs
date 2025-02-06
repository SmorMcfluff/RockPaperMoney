using UnityEngine;

public class FactoryStore : MonoBehaviour
{
    public static FactoryStore Instance;

    [HideInInspector] public float baseFactoryPrice = 0.01f;
    [HideInInspector] public float factoryPrice;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        Instance.SetFactoryPrice();
    }


    public static void PurchaseFactory()
    {
        if (Instance.CheckIfAffordable())
        {
            SaveDataManager.Instance.localPlayerData.ChangeMoneyBalance(-Instance.factoryPrice);
            IdleGameManager.Instance.AddFactory();

            IdleGameUIManager.Instance.UpdateMoneyText();
            IdleGameUIManager.Instance.UpdatePriceText(Instance.CheckIfAffordable());
        }
    }


    public void SetFactoryPrice()
    {
        int factoryAmount = IdleGameManager.Instance.factories.Count;
        if (factoryAmount % 2 == 0)
        {
            factoryPrice = baseFactoryPrice * Mathf.Pow(3, factoryAmount * 0.5f);
        }
        else
        {
            factoryPrice = baseFactoryPrice * Mathf.Pow(3, (factoryAmount - 1) * 0.5f);
        }

        IdleGameUIManager.Instance.UpdatePriceText(CheckIfAffordable());
    }


    public bool CheckIfAffordable()
    {
        return SaveDataManager.Instance.localPlayerData.GetMoneyBalance() >= Instance.factoryPrice;
    }
}
