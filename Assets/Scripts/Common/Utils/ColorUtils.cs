using UnityEngine;

public static class ColorUtils {
    public static bool ColorEquals(Color color1, Color color2)
    {
        return ColorUtility.ToHtmlStringRGB(color1) == ColorUtility.ToHtmlStringRGB(color2);
    }
}
