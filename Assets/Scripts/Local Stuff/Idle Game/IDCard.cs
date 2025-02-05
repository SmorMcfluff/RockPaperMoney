using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.UI;

public class IDCard : MonoBehaviour
{
    [SerializeField] private Image body;
    [SerializeField] private Image hair;
    [SerializeField] private Image head;
    [SerializeField] private Image eyes;
    [SerializeField] private Image nose;
    [SerializeField] private Image mouth;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI sexText;
    [SerializeField] private TextMeshProUGUI ageText;


    public void Construct(AdWatcherInfo data)
    {
        nameText.text = $"{data.firstName} {data.lastName}";
        sexText.text = SetSexText(data.sex);
        ageText.text = $"{data.age}";
    }


    private string SetSexText(Sex sex)
    {
        if (sex == Sex.Male)
            return "M";
        return "F";
    }
}
