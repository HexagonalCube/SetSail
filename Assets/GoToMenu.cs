using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenu : MonoBehaviour
{
    private void Start()
    {
        GameAudioFade.Instance.AudioFadeOut();
        Invoke("LoadMenu", 2.5f);
    }
    void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
