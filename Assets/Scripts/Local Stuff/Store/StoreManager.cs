using System.Collections.Generic;
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

    [SerializeField] private GameObject[] storeCategories;

    [SerializeField] private Transform handSignsCategory;
    [SerializeField] private Transform skinsCategory;

    [SerializeField] private List<StoreItemUI> handSignItems;
    [SerializeField] private List<StoreItemUI> skinItems;


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
        GenerateStoreItems();

        SetCategoryButtonColor(currentCategory);
        SetCategoryButtonInteractable(currentCategory);

        ChangeCategory(1);
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

        storeCategories[(int)currentCategory].SetActive(false);
        storeCategories[buttonInt].SetActive(true);

        SetCategoryButtonColor(buttonCategory);
        SetCategoryButtonInteractable(buttonCategory);

        currentCategory = buttonCategory;
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
}