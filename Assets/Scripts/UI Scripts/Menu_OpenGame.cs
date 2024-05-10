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
        StartCoroutine(TimeWait());   
    }

    public void ExitConfirm()
    {
        Application.Quit();
    }

    IEnumerator TimeWait()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(1);
    }
}
