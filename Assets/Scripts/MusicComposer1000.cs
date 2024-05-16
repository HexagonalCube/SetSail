using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicComposer1000 : MonoBehaviour
{
    public static MusicComposer1000 Instance;
    [SerializeField] AudioClip[] musicPlucks;
    [SerializeField] AudioClip[] storyMusic;
    [SerializeField] AudioSource source;
    int curClip;
    int previousClip = 100;
    int previousClip2 = 100;
    bool plucks = false;
    bool fading = false;
    bool choosing = false;
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
        source = GetComponent<AudioSource>();
        //StartCoroutine(GameStart());
        StartStoryMusic(0);
    }
    private void Update()
    {
        //Debug.Log($"enabled{plucks} playing{source.isPlaying}");
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartPlucksMusic();
        }
        if (plucks && !source.isPlaying && !choosing)
        {
            Debug.Log("A");
            choosing = true;
            StartCoroutine(RandomizeTimeTracks(musicPlucks, 1f));
        }
    }
    public void StartStoryMusic(int index)
    {
        StartCoroutine(SoundFade(0.01f));
        StartCoroutine(WaitForNextTrack(storyMusic[index], 1f));
        source.loop = true;
        plucks = false;
    }
    public void StartPlucksMusic()
    {
        StartCoroutine(SoundFade(0.001f));
        source.loop = false;
        plucks = true;
    }
    public void FadeCurTrack(float strenght)
    {
        StartCoroutine(SoundFade(strenght));
    }
    IEnumerator GameStart()
    {
        yield return new WaitForSecondsRealtime(1f);
        //Debug.Log("A");
        StartStoryMusic(0);
    }
    IEnumerator SoundFade(float strenght, float i = 1)
    {
        //Debug.Log("B");
        fading = true;
        yield return new WaitForFixedUpdate();
        source.volume = Mathf.Lerp(0, source.volume, i);
        i -= strenght;
        Debug.Log($"{i} {strenght} {source.volume}");
        if (source.volume <= 0)
        {
            source.volume = 0;
            source.Stop();
            fading = false;
            //Debug.Log("STOP");
        }
        else
        {
            //Debug.Log("C");
            StartCoroutine(SoundFade(strenght, i));
        }
    }
    IEnumerator WaitForNextTrack(AudioClip clip, float volume)
    {
        //Debug.Log("Waiting");
        yield return new WaitWhile(() => source.isPlaying);
        yield return new WaitWhile(() => fading);
        //Debug.Log("GO");
        source.clip = clip;
        source.volume = volume;
        source.Play();
    }
    IEnumerator RandomizeTimeTracks(AudioClip[] clips, float volume)
    {
        yield return new WaitWhile(() => source.isPlaying);
        yield return new WaitForSecondsRealtime(Random.Range(5f, 15f));
        AudioClip clip = PickRandomClip(clips);
        StartCoroutine(WaitForNextTrack(clip, volume));
    }
    AudioClip PickRandomClip(AudioClip[] clips, int tries = 0)
    {
        tries++;
        Debug.Log($"is_previous {previousClip == curClip}/ is_repeating {curClip == previousClip2}/ i {curClip}/ tries {tries}");
        while (previousClip == curClip || curClip == previousClip2)
        {
            curClip = Random.Range(0,clips.Length);
        }
        if (curClip != previousClip && curClip != previousClip2 || tries > 6)
        {
            //Debug.Log(curClip + " " + tries);
            previousClip2 = previousClip;
            previousClip = curClip;
            choosing = false;
            return clips[curClip];
        }
        else
        {
            //Debug.Log(curClip + " " + tries);
            return PickRandomClip(clips, tries);
        }
    }
}
