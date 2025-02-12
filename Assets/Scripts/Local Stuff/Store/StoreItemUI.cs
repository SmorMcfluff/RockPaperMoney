using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemUI : MonoBehaviour
{
    public PurchasableObject purchasableObject;

    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button buyButton;

    [SerializeField] Image iconImg;

    public void SetUpItem()
    {
        priceText.text = $"${purchasableObject.price}";
        nameText.text = purchasableObject.name;

        purchasableObject.GetIcon();
        iconImg.sprite = purchasableObject.icon;

        SetColor();

        if(purchasableObject is Skin)
        {
            SetUpPreviewButton();
        }
    }


    private void SetColor()
    {
        PlayerData player = SaveDataManager.Instance.localPlayerData;
        if (!purchasableObject.IsAffordable(player))
        {
            buyButton.image.color = Color.gray;
        }
    }


    private void SetUpPreviewButton()
    {
        SkinType skin = (purchasableObject as Skin).skin;
        GetComponent<PreviewButton>().AddListener(skin);
    }


    public void BuyItem()
    {
        purchasableObject.GetBought();
        StoreManager.Instance.GenerateStoreItems();
    }
}