using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_SFX : MonoBehaviour
{
    //audio output
    [SerializeField] AudioSource audioSource;

    //imported audio files
    [SerializeField] AudioClip importantClick;
    [SerializeField] AudioClip positiveClick;
    [SerializeField] AudioClip positiveHover;
    [SerializeField] AudioClip negativeClick;
    [SerializeField] AudioClip negativeHover;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ImportanteClick()
    {
        audioSource.PlayOneShot(importantClick);
    }

    public void PositiveClick()
    {
        audioSource.PlayOneShot(positiveClick);
    }

    public void PositiveHover()
    {
        audioSource.PlayOneShot(positiveHover);
    }

    public void NegativeClick()
    {
        audioSource.PlayOneShot(negativeClick);
    }

    public void NegativeHover()
    {
        audioSource.PlayOneShot(negativeHover);
    }

}
