using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Navigation : MonoBehaviour
{
    public static UI_Navigation Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }
    public void StoryButtonClick()
    {
        PauseMenuScript.Instance.OnStoryButtonClick();
    }
    public void ConfigButtonClick()
    {
        PauseMenuScript.Instance.OnConfigButtonClick();
    }
    public void ControlsButtonClick()
    {
        PauseMenuScript.Instance.OnControlsButtonClick();
    }
    public void HomeButtonClick()
    {
        PauseMenuScript.Instance.OnMenuButtonClick();
    }
    public void QuitButtonClick()
    {
        PauseMenuScript.Instance.OnMenuButtonClick();
    }
}
