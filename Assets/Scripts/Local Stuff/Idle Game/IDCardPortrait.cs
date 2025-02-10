using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class IDCardPortrait : MonoBehaviour
{
    [SerializeField] private Image body;
    [SerializeField] private Image hair;
    [SerializeField] private Image head;
    [SerializeField] private Image eyes;
    [SerializeField] private Image nose;
    [SerializeField] private Image mouth;

    [SerializeField] private Image[] wrinkles;

    [SerializeField] private List<IDComponent> idComponents;


    public void GeneratePortrait(AdWatcherInfo data)
    {
        var appearance = data.appearance;

        ResetWrinkles();

        foreach (var component in idComponents)
        {
            switch (component.name)
            {
                case "Body":
                    body.sprite = component.variations[appearance.bodyIndex];
                    body.color = ColorHelper.HexToRGB(appearance.bodyColorHex);
                    break;

                case "Hair":
                    SetHairOrder(appearance);
                    hair.sprite = component.variations[appearance.hairIndex];
                    hair.color = ColorHelper.HexToRGB(appearance.hairColorHex);
                    break;

                case "Head":
                    head.sprite = component.variations[appearance.headIndex];
                    head.color = ColorHelper.HexToRGB(appearance.skinColorHex);
                    break;

                case "Eyes":
                    eyes.sprite = component.variations[appearance.eyesIndex];
                    eyes.color = ColorHelper.HexToRGB(appearance.eyeColorHex);
                    break;

                case "Nose":
                    nose.sprite = component.variations[appearance.noseIndex];
                    break;

                case "Mouth":
                    mouth.sprite = component.variations[appearance.mouthIndex];
                    break;
            }
        }
        if(data.age >= 60)
        {
            SetWrinkles(data.age);
        }
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
        int index = 0;

        for(int i = 0; i <= agePastSixty; i++)
        {
            if (i % 5 == 0)
            {
                wrinkles[index].gameObject.SetActive(true);
                index++;
            }
        }
    }
}
