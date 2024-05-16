using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Navigation : MonoBehaviour
{
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
        PauseMenuScript.Instance.OnQuitButtonClick();
    }
    public void SwitchTabs(int tab)
    {
        switch (tab)
        {
            case 0:
                PauseMenuScript.Instance.On1Tab();
                break;
            case 1:
                PauseMenuScript.Instance.On2Tab();
                break;
            case 2:
                PauseMenuScript.Instance.On3Tab();
                break;
        }
    }
}
