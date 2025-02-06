using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemUI : MonoBehaviour
{
    public PurchasableObject purchasableObject;

    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] Image icon;


    private void Awake()
    {
        
    }

    public void BuyItem()
    {
        SaveDataManager.Instance.localPlayerData.ChangeMoneyBalance(-purchasableObject.price);
        StoreManager.Instance.GenerateStoreItems();
    }
}
