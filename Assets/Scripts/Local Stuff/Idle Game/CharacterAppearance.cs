using UnityEngine;

[System.Serializable]
public class CharacterAppearance
{
    public int bodyIndex = 0;
    public int hairIndex = 0;
    public int headIndex = 0;
    public int eyesIndex = 0;
    public int noseIndex = 0;
    public int mouthIndex = 0;


    public string bodyColorHex = "#ffffff";
    public string hairColorHex = "#ffffff";
    public string skinColorHex = "#ffffff";
    public string eyeColorHex = "#ffffff";


    public CharacterAppearance(AdWatcherInfo data)
    {
        if (data.lastName != "Mcfluff")
        {
            SetStyles(data.sex);
            SetColors(data.age);
        }
    }


    private void SetStyles(Sex sex)
    {
        bodyIndex = Random.Range(0, 4);
        hairIndex = SetHairStyle(sex);
        headIndex = Random.Range(0, 3);
        eyesIndex = Random.Range(0, 4);
        noseIndex = Random.Range(0, 4);
        mouthIndex = Random.Range(0, 4);
    }


    private int SetHairStyle(Sex sex)
    {
        if (sex == Sex.Female)
        {
            return Random.Range(0, 4);
        }
        else
        {
            return Random.Range(4, 8);
        }
    }


    private void SetColors(int age)
    {
        bodyColorHex = GetRandomColorHex();
        hairColorHex = GetRandomColorHex();
        skinColorHex = SetSkinColorHex(age);
        eyeColorHex = SetEyeColorHex();
    }


    private string SetSkinColorHex(int age)
    {
        Color[] colors = new Color[]
        {
            ColorHelper.HexToRGB("#8d5524"),
            ColorHelper.HexToRGB("#c68642"),
            ColorHelper.HexToRGB("#e0ac69"),
            ColorHelper.HexToRGB("#f1c27d"),
            ColorHelper.HexToRGB("#ffdbac")
        };

        var color = colors[Random.Range(0, colors.Length)];

        if (age >= 60)
        {
            MakePaler(ref color, age);
        }

        return ColorHelper.RGBToHex(color);
    }


    private void MakePaler(ref Color color, int age)
    {
        float ageOverSixty = age - 59;
        float maxAgeOverSixty = 20f;
        float lerpFactor = Mathf.Clamp01(ageOverSixty / maxAgeOverSixty) * 0.5f;
        color = Color.Lerp(color, Color.white, lerpFactor);
    }


    private string SetEyeColorHex()
    {
        Color[] colors = new Color[]
        {
            ColorHelper.HexToRGB("#634e34"),
            ColorHelper.HexToRGB("#2e536f"),
            ColorHelper.HexToRGB("#3d671d"),
            ColorHelper.HexToRGB("#1c7847"),
            ColorHelper.HexToRGB("#497665")
        };

        var color = colors[Random.Range(0, colors.Length)];

        return ColorHelper.RGBToHex(color);
    }


    private string GetRandomColorHex()
    {
        return ColorHelper.RGBToHex(Random.ColorHSV());
    }
}
