using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IDCardPortrait : MonoBehaviour
{
    [SerializeField] private Image body;
    [SerializeField] private Image hair;
    [SerializeField] private Image head;
    [SerializeField] private Image eyes;
    [SerializeField] private Image nose;
    [SerializeField] private Image mouth;
    [SerializeField] private Image adamsFace;

    [SerializeField] private Image[] wrinkles;

    [SerializeField] private List<IDComponent> idComponents;

    private Dictionary<string, System.Action<CharacterAppearance>> componentUpdaters;

    private void OnEnable()
    {
        if(componentUpdaters == null)
        {
            InitComponentUpdaters();
        }
    }


    private void InitComponentUpdaters()
    {
        componentUpdaters = new()
        {
           { "Body", UpdateBody },
            {"Hair", UpdateHair },
            {"Head", UpdateHead },
            {"Eyes", UpdateEyes },
            {"Nose", UpdateNose },
            {"Mouth", UpdateMouth }
        };
    }


    public void GeneratePortrait(AdWatcherInfo data)
    {
        if(componentUpdaters == null)
        {
            InitComponentUpdaters();
        }

        if (IsAdam(data))
        {
            adamsFace.enabled = true;
            return;
        }
        else if (adamsFace.enabled)
        {
            adamsFace.enabled = false;
        }


        var appearance = data.appearance;

        ResetWrinkles();

        foreach (var component in idComponents)
        {
            if(componentUpdaters.ContainsKey(component.name))
            {
                componentUpdaters[component.name](appearance);
            }
        }
        if (data.age >= 60)
        {
            SetWrinkles(data.age);
        }
    }


    private bool IsAdam(AdWatcherInfo data)
    {
        return data.lastName == "Mcfluff";
    }


    private void ResetWrinkles()
    {
        foreach (Image img in wrinkles)
        {
            img.gameObject.SetActive(false);
        }
    }


    private void SetHairOrder(CharacterAppearance appearance)
    {
        int targetIndex = (appearance.hairIndex == 0 || appearance.hairIndex == 1)
            ? 1
            : 2;

        hair.transform.SetSiblingIndex(targetIndex);
    }


    private void SetWrinkles(int age)
    {
        int agePastSixty = age - 60;
        int wrinkleCount = Mathf.FloorToInt(agePastSixty / 5);

        for (int i = 0; i < wrinkleCount; i++)
        {
            if (i < wrinkles.Length)
            {
                wrinkles[i].gameObject.SetActive(true);
            }
        }
    }

    private void UpdateBody(CharacterAppearance appearance)
    {
        body.sprite = idComponents[0].variations[appearance.bodyIndex];
        body.color = ColorHelper.HexToRGB(appearance.bodyColorHex);
    }


    private void UpdateHair(CharacterAppearance appearance)
    {
        SetHairOrder(appearance);
        hair.sprite = idComponents[1].variations[appearance.hairIndex];
        hair.color = ColorHelper.HexToRGB(appearance.hairColorHex);

    }


    private void UpdateHead(CharacterAppearance appearance)
    {
        head.sprite = idComponents[2].variations[appearance.headIndex];
        head.color = ColorHelper.HexToRGB(appearance.skinColorHex);
    }


    private void UpdateEyes(CharacterAppearance appearance)
    {
        eyes.sprite = idComponents[3].variations[appearance.eyesIndex];
        eyes.color = ColorHelper.HexToRGB(appearance.eyeColorHex);
    }


    private void UpdateNose(CharacterAppearance appearance)
    {
        nose.sprite = idComponents[4].variations[appearance.noseIndex];

    }


    private void UpdateMouth(CharacterAppearance appearance)
    {
        mouth.sprite = idComponents[5].variations[appearance.mouthIndex];

    }
}
