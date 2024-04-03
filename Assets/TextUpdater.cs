using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    TMP_Text text;
    Animator anim;
    public string VisibleText { private get { return text.text; } set { text.text = value; anim.Play("TextCommentPopup"); } }
    private void Start()
    {
        text = GetComponent<TMP_Text>();
        anim = GetComponent<Animator>();
        VisibleText = "";
    }
}
