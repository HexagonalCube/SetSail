using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Animator anim;
    public string VisibleText { private get { return text.text; } set { text.text = value; anim.Play("TextCommentPopup"); } }
    private void Start()
    {
        VisibleText = "";
    }
}
