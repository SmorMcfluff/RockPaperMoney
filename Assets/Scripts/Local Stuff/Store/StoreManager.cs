using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;

    [SerializeField] private Transform handSignsCategory;
    [SerializeField] private Transform skinsCategory;

    [SerializeField] private GameObject storeItemUIPrefab;
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
            GenerateStoreItems();
        }
    }

    public void GenerateStoreItems()
    {
        foreach (PurchasableObject po in purchasableObjects)
        {
            GameObject newObj = (po is Skin)
                ? Instantiate(storeItemUIPrefab, skinsCategory)
                : Instantiate(storeItemUIPrefab, handSignsCategory);

            newObj.GetComponent<StoreItemUI>().purchasableObject = po;
        }
    }
}
