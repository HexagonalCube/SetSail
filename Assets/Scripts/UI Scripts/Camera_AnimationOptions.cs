using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_AnimationOptions : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject panelConfigs;
    [SerializeField] GameObject panelCredits;
    [SerializeField] GameObject panelMenu;

    public void GoToCredits()
    {
        StartCoroutine(WaitForTime(true));
    }

    public void ReturnFromCredits()
    {
        StartCoroutine(WaitForTime(false));
    }

    public void GoToSettings()
    {
        StartCoroutine(WaitTime(true));
    }

    public void ReturnFromSettings()
    {
        StartCoroutine(WaitTime(false));
    }

    IEnumerator WaitTime(bool enable)
    {
        animator.SetBool("Configuracoes", enable);

        if (enable)
        {
            panelMenu.SetActive(!enable);
            yield return new WaitForSeconds(1f);
            panelConfigs.SetActive(enable);  

        } else
        {
            panelConfigs.SetActive(enable);
            yield return new WaitForSeconds(1f);
            panelMenu.SetActive(!enable);
        }
    }

    IEnumerator WaitForTime(bool enable)
    {
        animator.SetBool("Creditos", enable);

        if (enable)
        {
            panelMenu.SetActive(!enable);
            yield return new WaitForSeconds(1f);
            panelCredits.SetActive(enable);

        }
        else
        {
            panelCredits.SetActive(enable);
            yield return new WaitForSeconds(1f);
            panelMenu.SetActive(!enable);
        }
    }


}
