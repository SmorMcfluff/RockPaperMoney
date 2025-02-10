using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemUI : MonoBehaviour
{
    public PurchasableObject purchasableObject;

    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button buyButton;

    [SerializeField] Image icon;

    public void SetUpItem()
    {
        priceText.text = $"${purchasableObject.price}";
        nameText.text = purchasableObject.name;

        if (purchasableObject is HandSignPurchasable handSignItem)
        {
            handSignItem.GetIcon();
        }

        icon.sprite = purchasableObject.icon;
        SetColor();
    }


    public void SetColor()
    {
        PlayerData player = SaveDataManager.Instance.localPlayerData;
        if (!purchasableObject.IsAffordable(player))
        {
            buyButton.image.color = Color.gray;
        }
    }


    public void BuyItem()
    {
        purchasableObject.GetBought();
        StoreManager.Instance.GenerateStoreItems();
    }
}