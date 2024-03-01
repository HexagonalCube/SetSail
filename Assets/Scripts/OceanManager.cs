using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Simulates the Water Shader and returns a height value at a specific point in the water and time
/// </summary>
public class OceanManager : MonoBehaviour
{
    public float waveHeight = 7f;
    public float waveSpeed = 7f;
    public float wavesFrequency = 4f;

    public Transform ocean;

    [SerializeField] Material oceanMat;

    Texture2D wavesDisplacement;

    void Start()
    {
        SetVariables();
    }
    void SetVariables() //Always have set variables
    {
        oceanMat = ocean.GetComponent<Renderer>().sharedMaterial;
        wavesDisplacement = (Texture2D)oceanMat.GetTexture("_WavesDisplacement");
    }
    public float WaterHeightAtPosition(Vector3 position) //Calculate Water Height at Pos
    {
        //Escalonamento velho
        //return ocean.position.y + wavesDisplacement.GetPixelBilinear(position.x * wavesFrequency / 100, position.z * wavesFrequency / 100 + Time.time * waveSpeed / 100).g * waveHeight / 100 * ocean.localScale.x;
        return ocean.position.y + wavesDisplacement.GetPixelBilinear(position.x * (wavesFrequency/100) * ocean.localScale.x, (position.z * (wavesFrequency/100) + Time.time * (waveSpeed/100)) * ocean.localScale.z).g * (waveHeight/100);
    }
    private void OnValidate() //Update variables in editor
    {
        if (!oceanMat)
        {
            SetVariables();
        }
        UpdateMaterial();
    }
    
    public void UpdateMaterial() //Update Materials in editor
    {
        oceanMat.SetFloat("_WavesFrequency", wavesFrequency / 100);
        oceanMat.SetFloat("_WavesSpeed", waveSpeed / 100);
        oceanMat.SetFloat("_WavesHeight", waveHeight / 100);
    }
}
