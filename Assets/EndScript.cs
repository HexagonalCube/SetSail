using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndScript : MonoBehaviour
{
    public static EndScript Instance;
    [SerializeField] GameObject[] objs;
    [SerializeField] VideoPlayer video;
    bool canQuitFlag = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }

    public void StartEndSequence()
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(true);
        }
        StartCoroutine(WaitForAnimationFinished());
    }
    public IEnumerator WaitForAnimationFinished()
    {
        yield return new WaitForSecondsRealtime(2f);
        yield return new WaitUntil(delegate { return !video.isPlaying; });
        yield return new WaitForSecondsRealtime(2f);
        video.Stop();
        video.time = video.length - 1;
        canQuitFlag = true;
    }
    void QuitSequence()
    {

    }
    private void Update()
    {
        if (canQuitFlag && Input.anyKeyDown)
        {
            QuitSequence();
        }
    }
}
