using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    public float waveHeight = 7f;
    public float waveSpeed = 7f;
    public float wavesFrequency = 4f;

    public Transform ocean;

    Material oceanMat;

    Texture2D wavesDisplacement;

    void Start()
    {
        SetVariables();
    }
    void SetVariables()
    {
        oceanMat = ocean.GetComponent<Renderer>().sharedMaterial;
        wavesDisplacement = (Texture2D)oceanMat.GetTexture("_WavesDisplacement");
    }
    public float WaterHeightAtPosition(Vector3 position)
    {
        return ocean.position.y + wavesDisplacement.GetPixelBilinear(position.x * wavesFrequency / 100, position.z * wavesFrequency / 100 + Time.time * waveSpeed / 100).g * waveHeight / 100 * ocean.localScale.x;
    }
    private void OnValidate()
    {
        if (!oceanMat)
        {
            SetVariables() ;
        }
        UpdateMaterial();
    }
    void UpdateMaterial()
    {
        oceanMat.SetFloat("_WavesFrequency", wavesFrequency / 100);
        oceanMat.SetFloat("_WavesSpeed", waveSpeed / 100);
        oceanMat.SetFloat("_WavesHeight", waveHeight / 100);
    }
}
