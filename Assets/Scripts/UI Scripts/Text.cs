using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Text : CustomUIComponent
{
    //referencing classes and enum
    public TextSO textData;
    public Style style;

    private TextMeshProUGUI textMeshProUGUI;


    //getting components for individual references
    public override void Setup()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    //read values from scriptable object
    public override void Configure()
    {
        textMeshProUGUI.color = textData.theme.GetTextColor(style);
        textMeshProUGUI.font = textData.font;
        textMeshProUGUI.fontSize = textData.size;
    }
}
