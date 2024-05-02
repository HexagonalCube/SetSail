using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_AnimationOptions : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject panelMenu;

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
            panel.SetActive(enable);  

        } else
        {
            panel.SetActive(enable);
            yield return new WaitForSeconds(1f);
            panelMenu.SetActive(!enable);
        }
    }

}
