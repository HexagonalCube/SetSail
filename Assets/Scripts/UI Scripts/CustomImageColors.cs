using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomImageColors : CustomUIComponent
{
    public ThemeSO theme;
    public Style style;

    //reads as the image component
    private Image image;

    public override void Setup()
    {
        image = GetComponentInChildren<Image>();
    }

    public override void Configure()
    {
        image.color = theme.GetColorPalette(style);  
    }

}
