using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls weather conditions
/// </summary>
public class WeatherController : MonoBehaviour
{
    [SerializeField] CloudManager clouds;
    [SerializeField] OceanManager ocean;
    [SerializeField] ParticleSystem rain;
    [Range(0f, 2f)]
    public float weather;
    public bool isRain;
    [SerializeField] Color baseColor;
    [SerializeField] Color stormColor;
    private void OnValidate() //Update in editor when values change
    {
        UpdateWeather();
    }
    void UpdateWeather() //Update managers to set values
    {
        UpdateOcean();
        UpdateClouds();
        UpdateRain();
    }
    void UpdateClouds() //DO NOT TOUCH UNLESS CHANGING VISUAL SETTINGS
    {
        clouds.cloudScale = 230 / (1+weather);
        clouds.cloudPower = 90 / (1+weather);
        clouds.cloudHeight = 30 * (2+weather);
        clouds.cloudSpeed = new Vector2 (weather*10+1, weather*10+1);
        clouds.cloudColor = Color.Lerp(baseColor, stormColor, weather);
        clouds.colorIntensity = 4 / (1+weather);
        clouds.UpdateMaterial();
    }
    void UpdateOcean() //DO NOT TOUCH UNLESS CHANGING VISUAL SETTINGS
    {
        ocean.waveHeight = 600 + (200*weather);
        ocean.wavesFrequency = 1.5f + (0.5f*weather);
        ocean.waveSpeed = 3 + (2*weather);
        ocean.UpdateMaterial();
    }
    void UpdateRain()
    {
        switch (isRain)
        {
            case true:
                rain.Play();
                break;
            case false:
                rain.Stop();
                break;
        }
        var emmision = rain.emission;
        emmision.rateOverTime = 33 * weather;
    }
}
