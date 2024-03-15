using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI_Controller : MonoBehaviour
{
    [SerializeField] TMP_Text interactionText;
    [SerializeField] GameObject controlsHud;
    float fade = 0f;
    bool fading = false;
    public bool scheduleFadeOut;
    private void Start()
    {
        interactionText.color = new Color(1, 1, 1, 0);
    }
    public void Interact(bool on)
    {
        if (on && !fading)
        {
            fading = true;
            StopAllCoroutines();
            //Debug.Log("ACTIVE");
            StartCoroutine(ColorFadeIn());
        }
        
        if (!on && !fading) 
        {
            fading = true;
            StopAllCoroutines();
            //Debug.Log("NOT");
            StartCoroutine(ColorFadeOut());
        }
    }
    private void Update()
    {
        if(scheduleFadeOut)
        {
            StopAllCoroutines();
            scheduleFadeOut = false;
            StartCoroutine(ColorFadeOut());
        }
    }
    void FadeAdd(float i) { fade += i; }
    void FadeSub(float i) { fade -= i; }
    private IEnumerator ColorFadeIn()
    {
        if(fade <= 1f)
        {
            FadeAdd(0.1f);
            interactionText.color = new Color(1, 1, 1, fade);
            yield return new WaitForSeconds(0.02f);
            StartCoroutine(ColorFadeIn());
        }
        else
        {
            yield return new WaitForSeconds(0.02f);
            fading = false;
        }
    }
    private IEnumerator ColorFadeOut()
    {
        if (fade >= 0f)
        {
            FadeSub(0.1f);
            interactionText.color = new Color(1, 1, 1, fade);
            yield return new WaitForSeconds(0.02f);
            StartCoroutine(ColorFadeOut());
        }
        else
        {
            yield return new WaitForSeconds(0.02f);
            fading = false;
        }
    }
}
