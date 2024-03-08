using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandsDistanceController : MonoBehaviour
{
    [SerializeField] Transform[] Islands;
    [SerializeField] float[] distances;
    [SerializeField] Transform boat;
    [SerializeField] protected float dist;
    private void Update()
    {
        for (int i = 0; i < Islands.Length; i++)
        {
            distances[i] = Vector3.Distance(boat.position, Islands[i].position);
        }
        dist = Mathf.Min(distances);
    }
}
