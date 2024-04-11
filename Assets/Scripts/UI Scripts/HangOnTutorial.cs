using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangOnTutorial : MonoBehaviour
{
    bool activeBuffer = false;
    private void OnEnable()
    {
        Debug.Log("A");
        if (activeBuffer)
        {
            TutorialScript.Instance.OpenTutorial();
        }
    }
    private void OnDisable()
    {
        activeBuffer = true;
        Debug.Log("B");
        TutorialScript.Instance.CloseTutorial();
    }
}
