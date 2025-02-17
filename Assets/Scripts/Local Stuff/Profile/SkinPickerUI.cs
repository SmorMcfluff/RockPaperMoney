using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinPickerUI : MonoBehaviour
{
    public Skin skin;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button equipButton;

    [SerializeField] Image iconImg;

    public void SetUpItem()
    {
        nameText.text = skin.name;

        skin.GetIcon();
        iconImg.sprite = skin.icon;

        SetColor();
        SetUpPreviewButton();
    }


    private void SetColor()
    {
        SkinType equippedSkin = SaveDataManager.Instance.localPlayerData.equippedSkin;

        equipButton.image.color = (skin.skin == equippedSkin) ? Color.gray : Color.white;
    }


    private void SetUpPreviewButton()
    {
        SkinType skinType = skin.skin;
        GetComponent<PreviewButton>().AddListener(skinType);
    }

    public void EquipSkin()
    {
        SaveDataManager.Instance.localPlayerData.equippedSkin = skin.skin;
        SaveDataManager.Instance.SavePlayer();
        SkinViewer.Instance.GenerateSkinList();
    }
}
