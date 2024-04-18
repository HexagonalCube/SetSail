using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookNoises : MonoBehaviour
{
    public static BookNoises Instance;
    [SerializeField] AudioClip[] sfx;
    [SerializeField] AudioSource source;
    public enum Noises { OpenBook, CloseBook, FlipPage, IndentPage, BumpBook, SnapPage, ScribblePage }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }

    public void PlayNoise(Noises noise)
    {
        switch (noise)
        {
            case Noises.OpenBook:
                source.PlayOneShot(sfx[0], 0.3f);
                break;
            case Noises.CloseBook:
                source.PlayOneShot(sfx[1]);
                break;
            case Noises.FlipPage:
                source.PlayOneShot(sfx[2], 0.5f);
                break;
            case Noises.IndentPage:
                source.PlayOneShot(sfx[3], 0.9f);
                break;
            case Noises.BumpBook:
                source.PlayOneShot(sfx[4]);
                break;
            case Noises.SnapPage:
                source.PlayOneShot(sfx[5], 0.6f);
                break;
            case Noises.ScribblePage:
                source.PlayOneShot(sfx[6], 0.5f);
                break;
        }
    }
}
