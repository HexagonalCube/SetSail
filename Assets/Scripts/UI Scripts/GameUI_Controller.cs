using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_Controller : MonoBehaviour
{
    public static GameUI_Controller Instance;
    [SerializeField] TMP_Text interactionText;
    [SerializeField] GameObject controlsHud;
    [SerializeField] Image cursor;
    [SerializeField] Animator cursorAnim;
    [SerializeField] Animator gameFade;
    [SerializeField] GameObject book;
    [SerializeField] PauseMenuScript bookP;
    [SerializeField] GamePause pause;
    //[SerializeField] TextUpdater interactText;
    [SerializeField] TextUpdater commentText;
    float textFade = 0f;
    float cursorFade = 0f;
    bool textFading = false;
    bool cursorFading = false;
    bool cursorSwitch;
    bool paused;
    bool stopInteractions = true;
    public bool CanInteract { get { return stopInteractions; } set { stopInteractions = !value; } }
    public bool IsPaused { get { return paused; } } 
    public bool scheduleFadeOut;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }
    private void Start()
    {
        cursorAnim = cursor.GetComponent<Animator>();
        interactionText.color = new Color(1, 1, 1, 0);
        cursor.color = new Color(1, 1, 1, 0);
    }
    void InitialSetup()
    {
        StartCoroutine(TutorialTimer());
    }
    private IEnumerator TutorialTimer()
    {
        yield return new WaitForSeconds(10f);
    }
    public void Comment(string text)
    {
        commentText.VisibleText = text;
    }
    public void Interact(bool on, string text = "")
    {
        interactionText.text = text;
        if (on && !textFading && !stopInteractions)
        {
            textFading = true;
            StopCoroutine(TextColorFadeOut());
            //Debug.Log("ACTIVE");
            StartCoroutine(TextColorFadeIn());
        }
        if (!on && !textFading && !stopInteractions)
        {
            textFading = true;
            StopCoroutine(TextColorFadeIn());
            //Debug.Log("NOT");
            StartCoroutine(TextColorFadeOut());
        }
    }
    public void CursorUpdate(bool open, bool active)
    {
        Debug.Log($"open {open}, active {active}");
        CursorScript.Instance.Cursor(active, open);
        //if (active && !cursorFading)
        //{
        //    cursorFading = true;
        //    StopCoroutine(CursorColorFadeOut());
        //    StartCoroutine(CursorColorFadeIn());
        //    if (open && open != cursorSwitch)
        //    {
        //        cursorAnim.Play("CursorClose");
        //    }
        //    else if (!open && open != cursorSwitch)
        //    {
        //        cursorAnim.Play("CursorOpen");
        //    }
        //    cursorSwitch = open;
        //}
        
        //if (!active && !cursorFading)
        //{
        //    cursorFading = true;
        //    StopCoroutine(CursorColorFadeIn());
        //    StartCoroutine(CursorColorFadeOut());
        //}
    }
    public void UI_Fade(float seconds)
    {
        StartCoroutine(GameFadeInOut(seconds));
    }
    public void OpenPause()
    {
        book.SetActive(true);
        bookP.OnConfigButtonClick();
        paused = true;
        pause.PauseGame();
    }
    public void OpenStory(int page = 1)
    {
        book.SetActive(true);
        bookP.OnStoryButtonClick();
        paused = true;
        pause.PauseGame();
        PageStoryController.Instance.DiscorverPages(page);
        PageSwitcher.Instance.SelectPage = page-1;
    }
    public void ClosePause()
    {
        book.SetActive(false);
        paused = false;
        //scheduleFadeOut = true;
        pause.ResumeGame();
    }
    private void Update()
    {
        if(scheduleFadeOut)
        {
            StopCoroutine(TextColorFadeOut());
            StopCoroutine(TextColorFadeIn());
            StopCoroutine(CursorColorFadeOut());
            StopCoroutine(CursorColorFadeIn());
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
            yield return new WaitForSecondsRealtime(0.02f);
            StartCoroutine(TextColorFadeIn());
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.02f);
            textFading = false;
        }
    }
    private IEnumerator TextColorFadeOut()
    {
        if (textFade >= 0f)
        {
            TextFadeSub(0.1f);
            interactionText.color = new Color(1, 1, 1, textFade);
            yield return new WaitForSecondsRealtime(0.02f);
            StartCoroutine(TextColorFadeOut());
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.02f);
            textFading = false;
        }
    }
    private IEnumerator CursorColorFadeIn()
    {
        if (cursorFade <= 1f)
        {
            CursorFadeAdd(0.1f);
            cursor.color = new Color(1, 1, 1, cursorFade);
            yield return new WaitForSecondsRealtime(0.02f);
            StartCoroutine(CursorColorFadeIn());
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.02f);
            cursorFading = false;
        }
    }
    private IEnumerator CursorColorFadeOut()
    {
        if (cursorFade >= 0f)
        {
            CursorFadeSub(0.1f);
            cursor.color = new Color(1, 1, 1, cursorFade);
            yield return new WaitForSecondsRealtime(0.02f);
            StartCoroutine(CursorColorFadeOut());
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.02f);
            cursorFading = false;
        }
    }
    private IEnumerator GameFadeInOut(float seconds)
    {
        gameFade.SetTrigger("Close");
        yield return new WaitForSecondsRealtime(seconds);
        gameFade.SetTrigger("Open");
    }
}
