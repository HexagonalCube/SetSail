using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_Controller : MonoBehaviour
{
    [SerializeField] TMP_Text interactionText;
    [SerializeField] GameObject controlsHud;
    [SerializeField] Image cursor;
    [SerializeField] Animator cursorAnim;
    float textFade = 0f;
    float cursorFade = 0f;
    bool textFading = false;
    bool cursorFading = false;
    bool cursorSwitch;
    public bool scheduleFadeOut;
    private void Start()
    {
        cursorAnim = cursor.GetComponent<Animator>();
        interactionText.color = new Color(1, 1, 1, 0);
        cursor.color = new Color(1, 1, 1, 0);
    }
    public void Interact(bool on)
    {
        if (on && !textFading)
        {
            textFading = true;
            StopCoroutine(TextColorFadeOut());
            //Debug.Log("ACTIVE");
            StartCoroutine(TextColorFadeIn());
        }
        
        if (!on && !textFading) 
        {
            textFading = true;
            StopCoroutine(TextColorFadeIn());
            //Debug.Log("NOT");
            StartCoroutine(TextColorFadeOut());
        }
    }
    public void CursorUpdate(bool open, bool active)
    {
        if (active && !cursorFading)
        {
            cursorFading = true;
            StopCoroutine(CursorColorFadeOut());
            StartCoroutine(CursorColorFadeIn());
            if (open && open != cursorSwitch)
            {
                cursorAnim.Play("CursorClose");
            }
            else if (!open && open != cursorSwitch)
            {
                cursorAnim.Play("CursorOpen");
            }
            cursorSwitch = open;
        }
        
        if (!active && !cursorFading)
        {
            cursorFading = true;
            StopCoroutine(CursorColorFadeIn());
            StartCoroutine(CursorColorFadeOut());
        }
    }
    private void Update()
    {
        if(scheduleFadeOut)
        {
            StopAllCoroutines();
            scheduleFadeOut = false;
            StartCoroutine(TextColorFadeOut());
            StartCoroutine(CursorColorFadeOut());
        }
    }
    void TextFadeAdd(float i) { textFade += i; }
    void TextFadeSub(float i) { textFade -= i; }
    void CursorFadeAdd(float i) { cursorFade += i; }
    void CursorFadeSub(float i) { cursorFade -= i; }
    private IEnumerator TextColorFadeIn()
    {
        if(textFade <= 1f)
        {
            TextFadeAdd(0.1f);
            interactionText.color = new Color(1, 1, 1, textFade);
            yield return new WaitForSeconds(0.02f);
            StartCoroutine(TextColorFadeIn());
        }
        else
        {
            yield return new WaitForSeconds(0.02f);
            textFading = false;
        }
    }
    private IEnumerator TextColorFadeOut()
    {
        if (textFade >= 0f)
        {
            TextFadeSub(0.1f);
            interactionText.color = new Color(1, 1, 1, textFade);
            yield return new WaitForSeconds(0.02f);
            StartCoroutine(TextColorFadeOut());
        }
        else
        {
            yield return new WaitForSeconds(0.02f);
            textFading = false;
        }
    }
    private IEnumerator CursorColorFadeIn()
    {
        if (cursorFade <= 1f)
        {
            CursorFadeAdd(0.1f);
            cursor.color = new Color(1, 1, 1, cursorFade);
            yield return new WaitForSeconds(0.02f);
            StartCoroutine(CursorColorFadeIn());
        }
        else
        {
            yield return new WaitForSeconds(0.02f);
            cursorFading = false;
        }
    }
    private IEnumerator CursorColorFadeOut()
    {
        if (cursorFade >= 0f)
        {
            CursorFadeSub(0.1f);
            cursor.color = new Color(1, 1, 1, cursorFade);
            yield return new WaitForSeconds(0.02f);
            StartCoroutine(CursorColorFadeOut());
        }
        else
        {
            yield return new WaitForSeconds(0.02f);
            cursorFading = false;
        }
    }
}
