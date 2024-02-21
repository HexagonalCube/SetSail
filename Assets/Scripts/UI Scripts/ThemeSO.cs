using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//quick asset creation
[CreateAssetMenu(menuName = "Custom UI/ThemeSO", fileName = "Theme")]

public class ThemeSO : ScriptableObject
{
    //general use of colors
    [Header("Color Palette")]
    public Color brown;
    public Color gold;
    public Color orange;
    public Color pomegranate;
    public Color carmine;
    public Color wine;

    [Header("Monochromatic")]
    public Color white;
    public Color gray;
    public Color black;

    //buttons color scheme
    [Header("Button Style 1")]
    public Color primary_bg;
    public Color primary_text;

    [Header("Button Style 2")]
    public Color secondary_bg;
    public Color secondary_text;

    [Header("Button Style 3")]
    public Color tertiary_bg;
    public Color tertiary_text;

    [Header("Other")]
    public Color disable;


    //checks for the parameters on the enum class and return the color's field
    public Color GetColorPalette(Style style)
    {
        if (style == Style.Brown)
        {
            return brown;
        }
        else if (style == Style.Gold)
        {
            return gold;
        }
        else if (style == Style.Orange)
        {
            return orange;
        }
        else if (style == Style.Pomegranate)
        {
            return pomegranate;
        }
        else if (style == Style.Carmine)
        {
            return carmine;
        }
        else if (style == Style.Wine)
        {
            return wine;
        }
        else if (style == Style.White)
        {
            return white;
        }
        else if (style == Style.Gray)
        {
            return gray;
        }
        else if (style == Style.Black)
        {
            return black;
        }

        return disable;
    }

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
