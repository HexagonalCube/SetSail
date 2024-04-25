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
        dist = Mathf.Pow(dist / 500, 8);
        weather.weather = Mathf.Clamp(dist / maxDist, 0, 2);
        if (dist > maxDist*1.2f && lastIsland) { weather.isRain = true; } else { weather.isRain = false; }
        weather.UpdateWeather();
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
