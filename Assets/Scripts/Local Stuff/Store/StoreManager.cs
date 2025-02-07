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

    [SerializeField] private GameObject[] storeCategories;

    [SerializeField] private Transform handSignsCategory;
    [SerializeField] private Transform skinsCategory;

    [SerializeField] private GameObject storeItemUIPrefab;
    private List<GameObject> storeItemUIs = new();

    [SerializeField] private List<PurchasableObject> purchasableObjects;


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
        SetCategoryButtonColor(currentCategory);
        SetCategoryButtonInteractable(currentCategory);

        GenerateStoreItems();
    }


    public void GenerateStoreItems()
    {
        ClearStoreItems();

        foreach (PurchasableObject po in purchasableObjects)
        {
            bool isSkin = (po is Skin);

            if (isSkin)
            {

            }
            else
            {
                int i = ((HandSignPurchasable)po).unlockedHandSignIndex;
                PlayerData player = SaveDataManager.Instance.localPlayerData;

                if (player.unlockedHandSigns[i])
                {
                    continue;
                }
            }

            GameObject newObj = Instantiate(storeItemUIPrefab, isSkin ? skinsCategory : handSignsCategory);
            storeItemUIs.Add(newObj);
            Debug.Log("new object created and added at index #" + (storeItemUIs.Count-1));

            var itemUI = newObj.GetComponent<StoreItemUI>();
            itemUI.purchasableObject = po;
            itemUI.SetUpItem();
        }
    }


    private void ClearStoreItems()
    {
        foreach (GameObject item in storeItemUIs)
        {
            Debug.Log("Clearing index #" + storeItemUIs.IndexOf(item));
            Destroy(item);
            Debug.Log("DESTROYED");
        }
        storeItemUIs.Clear();
        Debug.Log("List cleared");
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
