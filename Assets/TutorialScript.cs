using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] InputController input;
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] string[] text;
    [SerializeField] protected int selected = 0;
    [SerializeField] bool canProgress = true;

    private void Start()
    {
        tutorialText.SetText(text[0]);
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SwitchTutorialStage();
        }
    }
    public void TutorialStep()
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
            tutorialText.SetText(text[selected]);
            canProgress = true;
        }
    }
    void SwitchTutorialStage()
    {
        switch (selected)
        {
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
                if (Input.GetKeyDown(input.SailP) || Input.GetKeyDown(input.SailN)) { TutorialStep(); }
                break;
            case 4:
                if (Input.GetKeyDown(input.PInt)) { tutorialText.GetComponentInParent<Image>().gameObject.SetActive(false); }
                break;
            default: break;
        }
    }
}
