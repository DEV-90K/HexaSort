using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Utils
{
    public static Color ConvertToColor(string hexaCode)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hexaCode, out color);
        return color;
    }
}
