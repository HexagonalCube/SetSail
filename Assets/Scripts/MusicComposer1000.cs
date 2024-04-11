using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicComposer1000 : MonoBehaviour
{
    public static MusicComposer1000 Instance;
    [SerializeField] AudioClip[] musicPlucks;
    [SerializeField] AudioClip[] storyMusic;
    [SerializeField] AudioSource source;
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
    }
    private void FixedUpdate()
    {
        
    }
    public void StartStoryMusic()
    {
        SoundFade(0.01f);
        WaitForNextTrack(storyMusic[0], 0.3f);
    }
    void GameStart()
    {
        source.PlayOneShot(source.clip);
    }
    void SoundFade(float strenght)
    {
        for (float i = source.volume; i > 0;)
        {
            source.volume -= strenght;
            if (source.volume <= 0)
            {
                source.volume = 0;
                source.Stop();
            }
        }
    }
    IEnumerator WaitForNextTrack(AudioClip clip, float volume)
    {
        yield return new WaitWhile(() => source.isPlaying);
        source.clip = clip;
        source.volume = volume;
        source.Play();
    }
}
