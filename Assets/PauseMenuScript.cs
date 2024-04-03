using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    enum Panel { Config, Story, Tips }
    [SerializeField] GameUI_Controller ui;
    [SerializeField] GameObject configPanel;
    [SerializeField] GameObject storyPanel;

    public void OnCloseButtonClick()
    {
        ui.ClosePause();
    }
    public void OnMenuButtonClick()
    {
        Application.Quit();
    }
    public void OnConfigButtonClick()
    {
        PanelSwitcher(Panel.Config);
    }
    public void OnStoryButtonClick()
    {
        PanelSwitcher(Panel.Story);
    }
    void DisableAllPanels() //Disables all panels prior to activation
    {
        configPanel.SetActive(false);
        storyPanel.SetActive(false);
    }
    //NEED PAGES SWITCHER
    void PanelSwitcher(Panel page)
    {
        DisableAllPanels();
        switch (page)
        {
            case Panel.Config:
                configPanel.SetActive(true);
                break;
            case Panel.Story:
                storyPanel.SetActive(true);
                break;
            case Panel.Tips:

                break;
        }
    }
}
