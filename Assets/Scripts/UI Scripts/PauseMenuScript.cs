using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static PauseMenuScript Instance;
    enum Panel { Config, Story, Tips }
    [SerializeField] GameUI_Controller ui;
    [SerializeField] GameObject configPanel;
    [SerializeField] GameObject storyPanel;
    [SerializeField] GameObject tipsPanel;

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
        //Debug.Log("Hover");
        BookNoises.Instance.PlayNoise(BookNoises.Noises.SnapPage);
    }
    public void OnCloseButtonClick()
    {
        //Debug.Log("Close");
        BookNoises.Instance.PlayNoise(BookNoises.Noises.CloseBook);
        ui.ClosePause();
    }
    public void OnMenuButtonClick()
    {
        //Debug.Log("Menu");
        BookNoises.Instance.PlayNoise(BookNoises.Noises.IndentPage);
        StartCoroutine(GameFadeout(true));
    }
    public void OnConfigButtonClick()
    {
        //Debug.Log("Config");
        BookNoises.Instance.PlayNoise(BookNoises.Noises.IndentPage);
        PanelSwitcher(Panel.Config);
    }
    public void OnStoryButtonClick()
    {
        //Debug.Log("Story");
        BookNoises.Instance.PlayNoise(BookNoises.Noises.IndentPage);
        PanelSwitcher(Panel.Story);
    }
    public void OnControlsButtonClick()
    {
        //Debug.Log("Controls");
        BookNoises.Instance.PlayNoise(BookNoises.Noises.IndentPage);
        PanelSwitcher(Panel.Tips);
    }
    public void OnQuitButtonClick()
    {
        //Debug.Log("Quit");
        BookNoises.Instance.PlayNoise(BookNoises.Noises.IndentPage);
        StartCoroutine(GameFadeout(false));
    }
    void DisableAllPanels() //Disables all panels prior to activation
    {
        configPanel.SetActive(false);
        storyPanel.SetActive(false);
        tipsPanel.SetActive(false);
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
                tipsPanel.SetActive(true);
                break;
        }
    }
    public void On1Tab()
    {
        //Debug.Log("Tab1");
        PanelSwitcher(Panel.Story);
        PageSwitcher.Instance.SelectPage = 0;
    }
    public void On2Tab()
    {
        //Debug.Log("Tab3");
        PanelSwitcher(Panel.Story);
        PageSwitcher.Instance.SelectPage = 2;
    }
    public void On3Tab()
    {
        //Debug.Log("Tab2");
        PanelSwitcher(Panel.Story);
        PageSwitcher.Instance.SelectPage = 4;
    }
    IEnumerator GameFadeout(bool menu)
    {
        if (menu)
        {
            //Debug.Log("A");
            GameUI_Controller.Instance.UI_Fade(2f);
            yield return new WaitForSecondsRealtime(1.2f);
            SceneManager.LoadScene(0);
        }
        else
        {
            //Debug.Log("B");
            GameUI_Controller.Instance.UI_Fade(2f);
            yield return new WaitForSecondsRealtime(1.2f);
            Application.Quit();
        }
    }
}
