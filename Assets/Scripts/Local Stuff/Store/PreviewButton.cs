using UnityEngine;
using UnityEngine.UI;

public class PreviewButton : MonoBehaviour
{
    [SerializeField] private Button button;

    public void AddListener(SkinType skin)
    {
        button.onClick.AddListener(delegate { PreviewManager.Instance.ActivatePreview(skin); });
    }
}
