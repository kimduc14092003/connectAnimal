using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData
{
    public static Color32 ThemeColor;
    public static int limitTimeInLevel = 600;
    public static int limitTimeInEndless = 12;

    public static void SetDefaultValueThemeColor()
    {
        switch(PlayerPrefs.GetInt("bgSpriteIndex", 0))
        {
            case 0:
                ThemeColor = new Color32(177,96,250,255);
                break;
            case 1:
                ThemeColor = new Color32(73, 148, 239, 255);
                break;
        }
    }
}
