using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator textBoxAnimator;
    [SerializeField] GameObject startGameText;
    [SerializeField] GameObject boxText;

    bool canStart = false;
    private void Start()
    {
        boxText.SetActive(false);
        startGameText.SetActive(false);
        CameraScript.Instance.cameraActive = false;
        StartCoroutine(WaitForAnimationFinished());
    }
    private void Update()
    {
        if (Input.GetKeyDown(InputController.Instance.PInt)) { StartGame(); }
    }
    public IEnumerator WaitForAnimationFinished()
    {
        yield return new WaitUntil(delegate { return animator.GetCurrentAnimatorStateInfo(0).IsName("GameStartIdle"); });
        yield return new WaitForSecondsRealtime(2f);
        EnableStartGame();
    }
    void EnableStartGame()
    {
        boxText.SetActive(true);
        startGameText.SetActive(true);
        canStart = true;
    }
    void StartGame()
    {
        if (canStart)
        {
            MusicComposer1000.Instance.FadeCurTrack(0.001f);
            CameraScript.Instance.cameraActive = true;
            canStart = false;
            animator.SetTrigger("FadeOut");
            textBoxAnimator.SetTrigger("TextBoxFinal");
            startGameText.GetComponent<Animator>().SetTrigger("SolidText");
            //startGameText.SetActive(false);
            InputController.Instance.EnableInput = true;
            Destroy(this);
        }
    }
}
