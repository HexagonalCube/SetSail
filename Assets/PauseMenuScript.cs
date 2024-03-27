using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameUI_Controller ui;

    public void OnCloseButtonClick()
    {
        ui.ClosePause();
    }
    public void OnMenuButtonClick()
    {
        Application.Quit();
    }
    void OnConfigButtonClick()
    {

    }
}
