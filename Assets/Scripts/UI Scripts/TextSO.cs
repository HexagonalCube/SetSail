using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Custom UI/TextSO", fileName = "Text")]

public class TextSO : ScriptableObject
{
    public ThemeSO theme;

    public TMP_FontAsset font;
    public float size;
}
