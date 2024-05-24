using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_OpenGame : MonoBehaviour
{
    [SerializeField] GameObject panelFade;
    [SerializeField] Animator animator;

    public void StartGame()
    {
        panelFade.SetActive(true);
        animator.SetBool("FinalFade", true);
        StartCoroutine(WaitEnterGame());
        GameAudioFade.Instance.AudioFadeOut();
    }
    public void ExitConfirm()
    {
        panelFade.SetActive(true);
        animator.SetBool("FinalFade", true);
        StartCoroutine(WaitExitGame());
        GameAudioFade.Instance.AudioFadeOut();
    }

    IEnumerator WaitEnterGame()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(1);
    }
    IEnumerator WaitExitGame()
    {
        yield return new WaitForSeconds(4f);
        Application.Quit();
    }
}
