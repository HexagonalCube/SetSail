using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : CustomUIComponent
{
    //referencing classes
    public ViewSO viewData;

    public GameObject containerTop;
    public GameObject containerCenter;
    public GameObject containerBottom;

    private Image _imageTop;
    private Image _imageCenter;
    private Image _imageBottom;

    private VerticalLayoutGroup _verticalLayoutGroup;


    //getting components for individual references
    public override void Setup()
    {
        _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        _imageTop = containerTop.GetComponent<Image>();
        _imageCenter = containerCenter.GetComponent<Image>();
        _imageBottom = containerBottom.GetComponent<Image>();
    }

    //read values from scriptable object
    public override void Configure()
    {
        _verticalLayoutGroup.padding = viewData.padding;
        _verticalLayoutGroup.spacing = viewData.spacing;

        _imageTop.color = viewData.theme.primary_bg;
        _imageCenter.color = viewData.theme.secondary_bg;
        _imageBottom.color = viewData.theme.tertiary_bg;
    }

}
