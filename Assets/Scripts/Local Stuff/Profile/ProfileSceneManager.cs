using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSceneManager : MonoBehaviour
{
    public static ProfileSceneManager Instance;

    public enum ProfileCategory { Skins, Stats, Upgrades }
    public ProfileCategory currentCategory;

    public Button skinsButton;
    public Button statsButton;
    public Button upgradesButton;

    [SerializeField] private GameObject[] profileCategoryPanels;
    [SerializeField] private TextMeshProUGUI balanceText;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateBalanceText();

        int selectedCategory = PlayerPrefs.GetInt("selectedProfileCategory", 1);
        if (selectedCategory < 0 || selectedCategory >= profileCategoryPanels.Length)
        {
            selectedCategory = 1;
        }
        currentCategory = (ProfileCategory)selectedCategory;

        foreach (GameObject panel in profileCategoryPanels)
        {
            panel.SetActive(false);
        }
        profileCategoryPanels[selectedCategory].SetActive(true);

        SetCategoryButtonColor(currentCategory);
        SetCategoryButtonInteractable(currentCategory);
    }

    public void ChangeCategory(int buttonInt)
    {
        ProfileCategory buttonCategory = (ProfileCategory)buttonInt;

        if (buttonCategory == currentCategory)
        {
            return;
        }

        for (int i = 0; i < profileCategoryPanels.Length; i++)
        {
            profileCategoryPanels[i].SetActive(i == buttonInt);
        }

        SetCategoryButtonColor(buttonCategory);
        SetCategoryButtonInteractable(buttonCategory);

        currentCategory = buttonCategory;

        PlayerPrefs.SetInt("selectedProfileCategory", buttonInt);
    }


    private void SetCategoryButtonColor(ProfileCategory category)
    {
        skinsButton.image.color = Color.white;
        statsButton.image.color = Color.white;
        upgradesButton.image.color = Color.white;

        switch (category)
        {
            case ProfileCategory.Skins:
                skinsButton.image.color = Color.green;
                break;
            case ProfileCategory.Stats:
                statsButton.image.color = Color.green;
                break;
            case ProfileCategory.Upgrades:
                upgradesButton.image.color = Color.green;
                break;
        }
    }


    private void SetCategoryButtonInteractable(ProfileCategory category)
    {
        statsButton.interactable = (category != ProfileCategory.Stats);
        skinsButton.interactable = (category != ProfileCategory.Skins);
        upgradesButton.interactable = (category != ProfileCategory.Upgrades);
    }


    public void UpdateBalanceText()
    {
        balanceText.text = "Balance: $" + SaveDataManager.Instance.localPlayerData.moneyBalance;
    }
}
