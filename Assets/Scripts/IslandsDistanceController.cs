using UnityEngine;

public class IslandsDistanceController : MonoBehaviour
{
    [SerializeField] Transform[] islands;
    [SerializeField] float[] distances;
    [SerializeField] Transform boat;
    [SerializeField] WeatherController weather;
    [SerializeField] float dist;
    private void Update()
    {
        distances = new float[islands.Length];
        for (int i = 0; i < islands.Length; i++)
        {
            distances[i] = Vector3.Distance(boat.position, islands[i].position);
        }
        dist = Mathf.Min(distances);
        dist = Mathf.Pow(dist / 500, 8);
        Debug.Log(dist);
        dist = 
        weather.weather = Mathf.Clamp(dist / 1000, 0, 2);
        if (dist > 750) { weather.isRain = true; } else { weather.isRain = false; }
        weather.UpdateWeather();
    }
}
