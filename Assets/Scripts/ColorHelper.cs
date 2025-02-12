using UnityEngine;

public class ColorHelper
{
    public static Color HexToRGB(string hexCode)
    {
        if (ColorUtility.TryParseHtmlString(hexCode, out Color color))
        {
            return color;
        }
        else
        {
            return Color.magenta;
        }
    }


    public static string RGBToHex(Color color)
    {
        return "#" + ColorUtility.ToHtmlStringRGB(color);
    }


    public static Color CombineColors(params Color[] colors)
    {
        Color result = Color.clear;

        foreach (Color color in colors)
        {
            result += color;
        }
        result /= colors.Length;
        return result;
    }
}
