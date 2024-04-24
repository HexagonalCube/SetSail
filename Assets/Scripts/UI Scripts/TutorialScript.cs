using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public static TutorialScript Instance;
    [SerializeField] InputController input;
    [SerializeField] Image panel;
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] string[] text;
    [SerializeField] protected int selected = 0;
    [SerializeField] bool canProgress = true;
    [SerializeField] protected bool isInTutorial = true;
    [SerializeField] bool hidden;
    public int TutProgress { get { return selected; } }
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
        tutorialText.SetText(text[0]);
        GameUI_Controller.Instance.CanInteract = !isInTutorial;
    }
    private void Update()
    {
        if (Input.anyKeyDown && isInTutorial)
        {
            SwitchTutorialStage();
        }
    }
    void TutorialStep()
    {
        if (canProgress)
        {
            canProgress = false;
            StartCoroutine(TutorialTimer());
        }
    }
    IEnumerator TutorialTimer()
    {
        yield return new WaitForSeconds(1f);
        if (selected < text.Length - 1)
        {
            selected++;
            tutorialText.SetText(hidden == false ? text[selected] : "");
            canProgress = true;
        }
    }
    public void CloseTutorial()
    {
        isInTutorial = false;
        HideShowTutorial(false);
    }
    public void OpenTutorial()
    {
        isInTutorial = true;
        HideShowTutorial(true);
    }
    public void HideShowTutorial(bool show)
    {
        hidden = !show;
        panel.enabled = show;
        tutorialText.SetText(show == true ? text[selected] : "");
    }
    void SwitchTutorialStage()
    {
        switch (selected)
        {
            default:
                break;
            case 0:
                if (Input.GetKeyDown(input.Pause) || Input.GetKeyDown(input.Diary)) { TutorialStep(); }
                break;
            case 1:
                if (Input.GetKeyDown(input.SailRL)) { TutorialStep(); }
                break;
            case 2:
                if (Input.GetKeyDown(input.RudderP) || Input.GetKeyDown(input.RudderN)) { TutorialStep(); }
                break;
            case 3:
                if (Input.GetKeyDown(input.SailP) || Input.GetKeyDown(input.SailN)) { TutorialStep(); HideShowTutorial(false); }
                break;
            case 4:
                if (Input.GetKeyDown(input.PInt) && !hidden) { CloseTutorial(); TutorialStep(); }
                break;
            case 5:
                if (Input.GetKeyDown(input.Navigation) && !hidden) { TutorialStep(); }
                break;
            case 6:
                if (Input.GetKeyDown(input.SailP) || Input.GetKeyDown(input.SailN) && !hidden) { CloseTutorial(); TutorialStep(); }
                break;
        }
    }
}
