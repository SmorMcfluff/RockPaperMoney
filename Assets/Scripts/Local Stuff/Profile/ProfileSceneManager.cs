using UnityEngine;
using UnityEngine.UI;

public class ProfileSceneManager : MonoBehaviour
{
    public enum ProfileCategory { Skins, Stats }
    public ProfileCategory currentCategory;

    public Button skinsButton;
    public Button statsButton;

    [SerializeField] private GameObject[] profileCategoryPanels;

    private void Start()
    {
        SetCategoryButtonColor(currentCategory);
        SetCategoryButtonInteractable(currentCategory);

        ChangeCategory(1);
    }

    public void ChangeCategory(int buttonInt)
    {
        ProfileCategory buttonCategory = (ProfileCategory)buttonInt;

        if (buttonCategory == currentCategory)
        {
            return;
        }

        profileCategoryPanels[(int)currentCategory].SetActive(false);
        profileCategoryPanels[buttonInt].SetActive(true);

        SetCategoryButtonColor(buttonCategory);
        SetCategoryButtonInteractable(buttonCategory);

        currentCategory = buttonCategory;
    }


    private void SetCategoryButtonColor(ProfileCategory category)
    {
        skinsButton.image.color = Color.white;
        statsButton.image.color = Color.white;

        if (category == ProfileCategory.Skins)
        {
            skinsButton.GetComponent<Image>().color = Color.green;
        }
        else if (category == ProfileCategory.Stats)
        {
            statsButton.GetComponent<Image>().color = Color.green;
        }
    }


    private void SetCategoryButtonInteractable(ProfileCategory category)
    {
        statsButton.interactable = (category != ProfileCategory.Stats);
        skinsButton.interactable = (category != ProfileCategory.Skins);
    }
}
