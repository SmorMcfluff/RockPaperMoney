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
            return Color.white;
        }
    }

    public static string RGBToHex(Color color)
    {
        return "#" + ColorUtility.ToHtmlStringRGB(color); 
    }
}
