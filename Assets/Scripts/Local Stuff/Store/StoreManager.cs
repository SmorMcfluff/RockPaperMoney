using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public enum StoreCategory { HandSigns, Skins }
    public StoreCategory currentCategory = StoreCategory.HandSigns;

    public static StoreManager Instance;

    public Button handSignButton;
    public Button skinButton;

    public GameObject storePanel;

    [SerializeField] private GameObject[] storeCategoryPanels;

    [SerializeField] private Transform handSignsCategory;
    [SerializeField] private Transform skinsCategory;

    [SerializeField] private List<StoreItemUI> handSignItems;
    [SerializeField] private List<StoreItemUI> skinItems;

    [SerializeField] private TextMeshProUGUI balanceText;


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
        UpdateBalanceText();

        int selectedCategory = PlayerPrefs.GetInt("selectedStoreCategory", 0);
        if (selectedCategory < 0 || selectedCategory >= storeCategoryPanels.Length)
        {
            selectedCategory = 0;
        }
        currentCategory = (StoreCategory)selectedCategory;

        SetCategoryButtonColor(currentCategory);
        SetCategoryButtonInteractable(currentCategory);

        ChangeCategory((int)currentCategory);
    }


    public void GenerateStoreItems()
    {
        ClearStoreItems();
        PlayerData player = SaveDataManager.Instance.localPlayerData;

        foreach (var item in handSignItems)
        {
            HandSignPurchasable handSignPurchase = (item.purchasableObject as HandSignPurchasable);

            int i = handSignPurchase.unlockedHandSignIndex;

            if (!player.unlockedHandSigns[i])
            {
                item.gameObject.SetActive(true);
                item.SetUpItem();
            }
        }

        foreach (var item in skinItems)
        {
            SkinType skinType = (item.purchasableObject as Skin).skin;

            if (!player.ownedSkins.Contains(skinType))
            {
                item.gameObject.SetActive(true);
                item.SetUpItem();
            }
        }
    }


    private void ClearStoreItems()
    {
        foreach (var item in handSignItems)
        {
            item.gameObject.SetActive(false);
        }

        foreach (var item in skinItems)
        {
            item.gameObject.SetActive(false);
        }
    }


    public void ChangeCategory(int buttonInt)
    {
        StoreCategory buttonCategory = (StoreCategory)buttonInt;

        if (buttonCategory == currentCategory)
        {
            return;
        }

        storeCategoryPanels[(int)currentCategory].SetActive(false);
        storeCategoryPanels[buttonInt].SetActive(true);

        SetCategoryButtonColor(buttonCategory);
        SetCategoryButtonInteractable(buttonCategory);

        currentCategory = buttonCategory;

        PlayerPrefs.SetInt("selectedStoreCategory", buttonInt);
    }


    private void SetCategoryButtonColor(StoreCategory category)
    {
        handSignButton.image.color = Color.white;
        skinButton.image.color = Color.white;

        if (category == StoreCategory.HandSigns)
        {
            handSignButton.GetComponent<Image>().color = Color.green;
        }
        else if (category == StoreCategory.Skins)
        {
            skinButton.GetComponent<Image>().color = Color.green;
        }
    }


    private void SetCategoryButtonInteractable(StoreCategory category)
    {
        handSignButton.interactable = (category != StoreCategory.HandSigns);
        skinButton.interactable = (category != StoreCategory.Skins);
    }


    public void UpdateBalanceText()
    {
        balanceText.text = "Balance: $" + SaveDataManager.Instance.localPlayerData.moneyBalance;

        if (PreviewManager.Instance.previewIsActive)
        {
            GenerateStoreItems();
        }
    }
}