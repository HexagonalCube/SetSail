using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailSfx : MonoBehaviour
{
    public static SailSfx Instance;
    [SerializeField] AudioClip sailOpen;
    [SerializeField] AudioClip sailClose;
    [SerializeField] AudioClip sailBump;
    [SerializeField] AudioClip[] sailSounds;
    [SerializeField] AudioClip previousClip;
    [SerializeField] AudioSource sailSource;
    [Range(0f, 1.5f)]
    [SerializeField] float volumeScale;
    bool sailing = false;
    float timeUntilNextClip;
    public bool Sailing {  get { return sailing; } set {  sailing = value; OpenCloseSailSFX(sailing); } }
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
        sailSource = GetComponent<AudioSource>();
        MuteForSeconds(2f);
    }
    private void FixedUpdate()
    {
        if (sailing)
        {
            switch (timeUntilNextClip)
            {
                case > 0:
                    timeUntilNextClip -= 0.1f;
                    break;
                case < 0:
                    timeUntilNextClip = 0f;
                    break;
                case 0:
                    AudioClip sound = GetClip(sailSounds);
                    timeUntilNextClip = sound.length + Random.Range(0f, 3f);
                    sailSource.pitch = Random.Range(0.8f, 1.2f);
                    sailSource.PlayOneShot(sound, volumeScale);
                    break;
            }
        }
    }
    IEnumerator MuteForSeconds(float time)
    {
        float previousVolume = sailSource.volume;
        sailSource.volume = 0;
        yield return new WaitForSecondsRealtime(time);
        sailSource.volume = previousVolume;
    }
    void OpenCloseSailSFX(bool open)
    {
        if (open)
        {
            FixPitch();
            sailSource.PlayOneShot(sailOpen, volumeScale);
        }
        else
        {
            FixPitch();
            sailSource.PlayOneShot(sailClose, volumeScale);
        }
    }
    public void HeadWind()
    {
        FixPitch();
        sailSource.PlayOneShot(sailBump, volumeScale);
    }
    void FixPitch()
    {
        sailSource.pitch = 1f;
    }
    AudioClip GetClip(AudioClip[] clipArray)
    {
        int attempts = 10;
        AudioClip selectedClip =
        clipArray[Random.Range(0, clipArray.Length)];
        while (selectedClip == previousClip && attempts > 0)
        {
            selectedClip =
            clipArray[Random.Range(0, clipArray.Length)];

            attempts--;
        }
        previousClip = selectedClip;
        return selectedClip;
    }
}
