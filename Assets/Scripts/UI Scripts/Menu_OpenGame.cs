using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_OpenGame : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1.0f;
    }
    public static void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public static void ExitConfirm()
    {
        Application.Quit();
    }

}
