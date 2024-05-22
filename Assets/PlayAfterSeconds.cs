using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAfterSeconds : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] float seconds;
    [SerializeField] bool random;
    private void OnValidate()
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
        }
    }
    private void Start()
    {
        source.Stop();
        if (random)
        {
            StartCoroutine(PlayAfter(Random.Range(0f, 7.5f)));
        }
        else
        {
            StartCoroutine(PlayAfter(seconds));
        }
    }
    IEnumerator PlayAfter(float sec)
    {
        yield return new WaitForSecondsRealtime(sec);
        source.Play();
        Destroy(this);
    }
}
