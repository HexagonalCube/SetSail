using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothController : MonoBehaviour
{
    [SerializeField] Cloth[] cloths;
    [SerializeField] Vector3 direction;
    [SerializeField] float scale = 1;
    public Vector3 WindDirection { get { return direction; } set { direction = value; } }
    private void FixedUpdate()
    {
        foreach (Cloth cloth in cloths)
        {
            cloth.externalAcceleration = direction * scale;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, direction * scale);
    }
}
