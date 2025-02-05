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

    [SerializeField] private List<IDComponent> idComponents;


    public void GeneratePortrait(CharacterAppearance appearance)
    {
        foreach (var component in idComponents)
        {
            switch (component.name)
            {
                case "Body":
                    body.sprite = component.variations[appearance.bodyIndex];
                    body.color = ColorHelper.HexToRGB(appearance.bodyColorHex);
                    break;

                case "Hair":
                    if (appearance.hairIndex == 0 || appearance.hairIndex == 1)
                    {
                        hair.transform.SetSiblingIndex(hair.transform.GetSiblingIndex() - 1);
                    }
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
    }
}
