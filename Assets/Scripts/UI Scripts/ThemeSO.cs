using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom UI/ThemeSO", fileName = "Theme")]

public class ThemeSO : ScriptableObject
{
    [Header("Primary")]
    public Color primary_bg;
    public Color primary_text;

    [Header("Secondary")]
    public Color secondary_bg;
    public Color secondary_text;

    [Header("Tertiary")]
    public Color tertiary_bg;
    public Color tertiary_text;

    [Header("Other")]
    public Color disable;


    public Color GetBackgroundColor(Style style)
    {
        if (style == Style.Primary)
        {
            return primary_bg;
        } else if (style == Style.Secondary)
        {
            return secondary_bg;
        } else if (style == Style.Tertiary)
        {
            return tertiary_bg;
        }

        return disable;
    }

    public Color GetTextColor(Style style)
    {
        if (style == Style.Primary)
        {
            return primary_text;
        }
        else if (style == Style.Secondary)
        {
            return secondary_text;
        }
        else if (style == Style.Tertiary)
        {
            return tertiary_text;
        }

        return disable;
    }

}
