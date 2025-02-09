using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IDCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI sexText;
    [SerializeField] private TextMeshProUGUI ageText;
    [SerializeField] private Image flag;

    [SerializeField] private IDComponent flags;

    public IDCardPortrait portrait;

    
    public void LoadData(AdWatcherInfo data)
    {
        portrait.GeneratePortrait(data.appearance);

        SetFlag(data.country);

        nameText.text = $"{data.firstName} {data.lastName}";
        sexText.text = SetSexText(data.sex);
        ageText.text = $"AGE: {data.age}";
    }


    private void SetFlag(Country country)
    {
        flag.sprite = flags.variations[(int)country];
    }


    private string SetSexText(Sex sex) => sex == Sex.Male ? "M" : "F";


    public void ToggleEnabled()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
