using System.Collections;
using UnityEngine;

public class IslandsDistanceController : MonoBehaviour
{
    public static IslandsDistanceController Instance;
    [SerializeField] Transform[] islands;
    [SerializeField] float[] distances;
    [SerializeField] Transform boat;
    [SerializeField] WeatherController weather;
    [SerializeField] public float dist;
    [SerializeField] float maxDist;
    [SerializeField] bool lastIsland = false;
    [SerializeField] float weatherToGive;
    [SerializeField] float lerpUp;
    [SerializeField] float lerpDown;
    static float t;
    public bool SetLastIsland {  get { return lastIsland; } set {  lastIsland = value; } }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }
    private void Update()
    {
        distances = new float[islands.Length];
        for (int i = 0; i < islands.Length; i++)
        {
            distances[i] = Vector3.Distance(boat.position, islands[i].position);
        }
        dist = Mathf.Min(distances);
        Debug.Log(dist);
        //weather.weather = Mathf.Clamp(dist / maxDist, 0, 2);
        if (dist>maxDist)
        {
            LerpWeatherUp(lerpUp);
            if (dist > maxDist * 1.2f && lastIsland) { weather.isRain = true; } else { weather.isRain = false; }
        }
        else
        {
            LerpWeatherDown(lerpDown);
        }
        weather.weather = weatherToGive;
        weather.UpdateWeather();
    }
    void LerpWeatherUp(float count)
    {
        if (weatherToGive < 2)
        {
            float a = 0;
            t += count * Time.deltaTime;
            a = Mathf.Log10(Mathf.Abs(t + 1));
            weatherToGive = Mathf.Lerp(0, 2, a);
        }
        //Debug.Log("A" + weatherToGive + " t" + t);
    }
    void LerpWeatherDown(float count)
    {
        //Debug.Log("B" + weatherToGive + " t" + t);
        if (weatherToGive > 0)
        {
            float a = 0;
            t -= count * Time.deltaTime;
            a = Mathf.Log10(Mathf.Abs(t + 1));
            weatherToGive = Mathf.Lerp(0, 2, a);
        }
    }
    private void OnDrawGizmos()
    {
        foreach (Transform t in islands)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(t.position, maxDist);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, maxDist * 1.2f);
        }
    }
}
