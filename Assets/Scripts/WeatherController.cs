using UnityEngine;
//using UnityEngine.Experimental.GlobalIllumination;
/// <summary>
/// Controls weather conditions
/// </summary>
public class WeatherController : MonoBehaviour
{
    [SerializeField] CloudManager clouds;
    [SerializeField] OceanManager ocean;
    [SerializeField] ParticleSystem rain;
    [SerializeField] Light sun;
    [SerializeField] Material skybox;
    [Range(0f, 2f)]
    public float weather;
    public bool isRain;
    [SerializeField] Color baseColor;
    [SerializeField] Color stormColor;
    public delegate void Weather();
    public Weather UpdateWeather;
    private void OnValidate() //Update in editor when values change
    {
        UpdateWeather += UpdateOcean;
        UpdateWeather += UpdateClouds;
        UpdateWeather += UpdateRain;
        UpdateWeather += UpdateSun;
        UpdateWeather += UpdateFog;
        UpdateWeather();
    }
    private void Start()
    {
        UpdateWeather += UpdateOcean;
        UpdateWeather += UpdateClouds;
        UpdateWeather += UpdateRain;
        UpdateWeather += UpdateSun;
        UpdateWeather += UpdateFog;
        UpdateWeather();
    }
    public void UpdateThisWeather()
    {
        UpdateWeather();
    }
    void UpdateClouds() //DO NOT TOUCH UNLESS CHANGING VISUAL SETTINGS
    {
        clouds.cloudScale = 230 / (1+weather);
        clouds.cloudPower = 90 / (1+weather);
        clouds.cloudHeight = 30 * (2+weather);
        clouds.cloudSpeed = new Vector2 (weather+1, weather+1);
        clouds.cloudColor = Color.Lerp(baseColor, stormColor, weather/2);
        clouds.colorIntensity = 4 / (1+weather);
        clouds.cloudSmooth = 0.4f - (weather/5);
        clouds.UpdateMaterial();
    }
    void UpdateOcean() //DO NOT TOUCH UNLESS CHANGING VISUAL SETTINGS
    {
        ocean.waveHeight = 600 + (100*weather);
        //ocean.wavesFrequency = 1.5f + (0.5f*weather);
        //ocean.waveSpeed = 4 + (3*weather);
        ocean.UpdateMaterial();
    }
    void UpdateRain() //Updates rain particles
    {
        var emmision = rain.emission;
        emmision.rateOverTime = 33 * weather;
        emmision.enabled = isRain;
    }
    void UpdateSun() //Updates the skybox & sun to reflect the weather
    {
        if (weather > 0.1f)
        {
            sun.intensity = 2f - (0.3f + weather * 1.5f);
            skybox.SetFloat("_Exposure", 1.3f * 1 / (weather * 2 + 1));
        }
        
    }
    void UpdateFog()
    {
        //C5EAFE clear
        //121216 foggy
        Color newcolor = Color.Lerp(Color.HSVToRGB(201f/360f, 22f/100f, 100f/100f), Color.HSVToRGB(240f/360f, 18f/100f, 9f/100f), weather / 2f);
        RenderSettings.fogColor = newcolor;
        //Debug.Log(newcolor);
    }
}
