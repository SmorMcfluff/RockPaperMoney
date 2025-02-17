using System.Collections.Generic;
using UnityEngine;

public class SkinViewer : MonoBehaviour
{
    public static SkinViewer Instance;

    [SerializeField] private List<SkinPickerUI> skinItems;

    public GameObject skinViewPanel;

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
        GenerateSkinList();
    }


    public void GenerateSkinList()
    {
        List<SkinType> ownedSkins = SaveDataManager.Instance.localPlayerData.ownedSkins;

        foreach (var skinItem in skinItems)
        {
            if (!ownedSkins.Contains(skinItem.skin.skin))
            {
                skinItem.gameObject.SetActive(false);
                continue;
            }

            skinItem.SetUpItem();
        }
    }
}
