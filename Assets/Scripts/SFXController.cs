using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public static SFXController Instance;
    [Header("GeneralSources")]
    [SerializeField] AudioSource sourceWaterSFX;
    [SerializeField] AudioSource sourceMiscSFX;
    [SerializeField] AudioSource sourceWindSFX;
    [Space]
    [Header("SFX clips")]
    [SerializeField] AudioClip wavesSoftSFX;
    [SerializeField] AudioClip wavesHardSFX;
    [SerializeField] AudioClip wavesSplashSFX;
    [SerializeField] AudioClip woodCreakSFX;
    [SerializeField] AudioClip windsSoftSFX;
    [Space]
    [Header("Special Loops")]
    [SerializeField] AudioSource WaterMoving;
    [SerializeField] AudioSource WindMoving;

    public bool inBoat = false;
    public bool sailRaised = false;
    public bool inStorm = false;

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
        StartCoroutine(RepeatInSeconds(windsSoftSFX.length, windsSoftSFX, 0.6f, 100));
    }
    private void Update()
    {
        float chance = Random.value;
        if (inBoat && chance > 0.6f && !sourceMiscSFX.isPlaying)
        {
            sourceMiscSFX.PlayOneShot(woodCreakSFX,1);
        }
    }
    IEnumerator RepeatInSeconds(float seconds, AudioClip clip, float volume, int id) //Navigating Noises
    {
        float seconds1 = seconds + Random.value;
        if (id >= 100)
        {
            sourceWindSFX.PlayOneShot(clip, volume);
            yield return new WaitForSeconds(seconds1);
            StartCoroutine(RepeatInSeconds(seconds, clip, volume, id));
        }
        else
        {
            sourceWaterSFX.PlayOneShot(clip, volume);
            yield return new WaitForSeconds(seconds1);
            if (id == 0 || inBoat)
            {
                if (!inStorm && id != 10) { StartCoroutine(RepeatInSeconds(seconds, clip, volume, id)); }
                else { StartCoroutine(RepeatInSeconds(seconds, clip, volume, id)); }
                if (inStorm && id == 5) { StartCoroutine(RepeatInSeconds(seconds, clip, volume, id)); }
            }
        }
    }
    public void EnterBoat()
    {
        inBoat = true;
        StartCoroutine(RepeatInSeconds(wavesSoftSFX.length, wavesSoftSFX, 0.6f, 1));
        StartCoroutine(RepeatInSeconds(wavesSplashSFX.length, wavesSplashSFX, 0.75f, 5));
    }
    public void StartStorm()
    {
        inStorm = true;
        StartCoroutine(RepeatInSeconds(wavesHardSFX.length, wavesSoftSFX, 0.3f, 10));
    }
    public void ExitBoat()
    {
        inBoat = false;
        sourceWaterSFX.Stop();
    }
    public void StopStorm()
    {
        inStorm = false;
    }
    public void Moving(bool yes, float speed)
    {
        //Set Volume to speed
        float i = Mathf.Clamp01(speed/75);
        WindMoving.volume = i;
        WaterMoving.volume = i;

        if (yes && inBoat)//If Propulsion Active
        {
            if (!WindMoving.isPlaying)
            {
                WindMoving.Play();
                WindMoving.loop = true;
            }
            if (!WaterMoving.isPlaying)
            {
                WaterMoving.Play();
                WaterMoving.loop = true;
            }
        }
        else
        {
            WindMoving.loop = false;
            WaterMoving.loop = false;
        }
    }
}
