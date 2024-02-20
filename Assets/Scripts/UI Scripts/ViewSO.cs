using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom UI/ViewSO", fileName = "ViewSO")]

public class ViewSO : ScriptableObject
{
    public ThemeSO theme;
    public RectOffset padding;
    public float spacing;
}
