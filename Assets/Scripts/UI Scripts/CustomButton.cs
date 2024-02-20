using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomButton : CustomUIComponent
{
    public ThemeSO theme;
    public Style style;
    public UnityEvent onClick;

    private Button button;
    private TextMeshProUGUI buttonText;


    //getting components for individual references
    public override void Setup()
    {
        button = GetComponentInChildren<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    //read values from scriptable object
    public override void Configure()
    {
        //getting a copy of the ColorBlock to be able to change it
        ColorBlock cb = button.colors;
        cb.normalColor = theme.GetBackgroundColor(style);
        button.colors = cb;

        buttonText.color = theme.GetTextColor(style);
    }

    //invokes the UnityEvent 
    public void OnClick()
    {
        onClick.Invoke();
    }

}
