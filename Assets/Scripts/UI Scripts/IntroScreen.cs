using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject startGameText;

    bool canStart = false;
    private void Start()
    {
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
            startGameText.SetActive(false);
            InputController.Instance.EnableInput = true;
            Destroy(this);
        }
    }
}
