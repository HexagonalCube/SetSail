using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameAudioFade : MonoBehaviour
{
    public static GameAudioFade Instance;
    [SerializeField] AudioMixer audioMixer;
    float volume;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }
    private void Start()
    {
        AudioFadeIn();
    }
    public void AudioFadeOut()
    {
        volume = 0f;
        StartCoroutine(AddToVolume(-1f));
    }
    public void AudioFadeIn()
    {
        volume = -80f;
        StartCoroutine(AddToVolume(1f));
    }
    IEnumerator AddToVolume(float cnt)
    {
        volume += cnt;
        audioMixer.SetFloat("GameVolume", volume);
        yield return new WaitForSecondsRealtime(0.01f);
        if (volume == 0f || volume == -80f) { StopAllCoroutines(); }
        else { StartCoroutine(AddToVolume(cnt)); }
    }
}
