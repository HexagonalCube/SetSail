using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        CameraScript.Instance.cameraActive = false;
        StartCoroutine(WaitForAnimationFinished());
    }
    public IEnumerator WaitForAnimationFinished()
    {
        yield return new WaitForSecondsRealtime(2f);
        yield return new WaitUntil(delegate { return !video.isPlaying; });
        //yield return new WaitForSecondsRealtime(2f);
        video.Stop();
        video.time = video.length - 1;
        canQuitFlag = true;
    }
    void QuitSequence()
    {
        SceneManager.LoadScene(2);
    }
    private void Update()
    {
        if (canQuitFlag && Input.GetKeyDown(InputController.Instance.Diary))
        {
            QuitSequence();
        }
    }
}
