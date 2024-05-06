using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public static PauseMenuScript Instance;
    enum Panel { Config, Story, Tips }
    [SerializeField] GameUI_Controller ui;
    [SerializeField] GameObject configPanel;
    [SerializeField] GameObject storyPanel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }
    public void CursorHover()
    {
        BookNoises.Instance.PlayNoise(BookNoises.Noises.SnapPage);
    }
    public void OnCloseButtonClick()
    {
        BookNoises.Instance.PlayNoise(BookNoises.Noises.CloseBook);
        ui.ClosePause();
    }
    public void OnMenuButtonClick()
    {
        BookNoises.Instance.PlayNoise(BookNoises.Noises.IndentPage);
    }
    public void OnConfigButtonClick()
    {
        BookNoises.Instance.PlayNoise(BookNoises.Noises.IndentPage);
        PanelSwitcher(Panel.Config);
    }
    public void OnStoryButtonClick()
    {
        BookNoises.Instance.PlayNoise(BookNoises.Noises.IndentPage);
        PanelSwitcher(Panel.Story);
    }
    public void OnControlsButtonClick()
    {
        BookNoises.Instance.PlayNoise(BookNoises.Noises.IndentPage);
    }
    public void OnQuitButtonClick()
    {
        Application.Quit();
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
    public void On1Tab()
    {
        PanelSwitcher(Panel.Story);
        PageSwitcher.Instance.SelectPage = 1;
    }
    public void On2Tab()
    {
        PanelSwitcher(Panel.Story);
        PageSwitcher.Instance.SelectPage = 3;
    }
    public void On3Tab()
    {
        PanelSwitcher(Panel.Story);
        PageSwitcher.Instance.SelectPage = 5;
    }
}
