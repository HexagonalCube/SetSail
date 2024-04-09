using System.Collections;
using UnityEngine;

public class Footsteps : GameStage
{
    public static Footsteps Instance;
    CheckTerrainTexture t;
    [SerializeField] AudioSource speaker;
    [Space]
    [SerializeField] AudioClip[] stoneClips;
    [SerializeField] AudioClip[] dirtClips;
    [SerializeField] AudioClip[] sandClips;
    [SerializeField] AudioClip[] grassClips;
    [SerializeField] AudioClip[] gravelClips;
    [Space]
    [SerializeField] float stepTimer;
    AudioClip previousClip;
    bool playing = false;
    public Terrain Terrain { get { return t.Terrain; } set { t.Terrain = value; } }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }
    void Start()
    {
        speaker = GetComponent<AudioSource>();
        t = GetComponent<CheckTerrainTexture>();
        GameProgression.Instance.Stage = WorldStage.Island2;
    }
    private void Update()
    {
        float i = Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"));
        if (i > 0.5f && !playing) { PlayFootstep(); }
    }
    IEnumerator steptimer()
    {
        playing = true;
        yield return new WaitForSeconds(stepTimer);
        playing = false;
    }
    void PlayFootstep()
    {
        StartCoroutine(steptimer());
        speaker.pitch = Random.Range(0.8f, 1f);
        t.GetTerrainTexture();
        float[] textureValues = t.TextureValues;
        if (GameProgression.Instance.Stage == WorldStage.Island1)
        {
            if (textureValues[0] > 0)
            {
                speaker.PlayOneShot(GetClip(stoneClips), textureValues[0]);
            }
            if (textureValues[1] > 0)
            {
                speaker.PlayOneShot(GetClip(grassClips), textureValues[1]);
            }
            if (textureValues[2] > 0)
            {
                speaker.PlayOneShot(GetClip(dirtClips), textureValues[2]);
            }
            if (textureValues[3] > 0)
            {
                speaker.PlayOneShot(GetClip(sandClips), textureValues[3]);
            }
        }
        if (GameProgression.Instance.Stage == WorldStage.Island2)
        {
            if (textureValues[0] > 0)
            {
                speaker.PlayOneShot(GetClip(sandClips), textureValues[0]);
            }
            if (textureValues[1] > 0)
            {
                speaker.PlayOneShot(GetClip(grassClips), textureValues[1]);
            }
            if (textureValues[2] > 0)
            {
                speaker.PlayOneShot(GetClip(dirtClips), textureValues[2]);
            }
            if (textureValues[3] > 0)
            {
                speaker.PlayOneShot(GetClip(stoneClips), textureValues[3]);
            }
        }
    }
    AudioClip GetClip(AudioClip[] clipArray)
    {
        int attempts = 3;
        AudioClip selectedClip =
        clipArray[Random.Range(0, clipArray.Length - 1)];
        while (selectedClip == previousClip && attempts > 0)
        {
            selectedClip =
            clipArray[Random.Range(0, clipArray.Length - 1)];

            attempts--;
        }
        previousClip = selectedClip;
        return selectedClip;
    }
}
