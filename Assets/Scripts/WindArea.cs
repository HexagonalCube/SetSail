using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Defines wind area and wind direction
/// </summary>
public class WindArea : MonoBehaviour
{
    public float Rotation; //WindDirection Rotation
    public Transform trDirection; //WindDirection transform
    public Vector3 direction; //Effective Wind pointer
    public Transform particles; //Particles object

    [SerializeField] BoxCollider box; //Effective Area
    private void OnValidate() //Always set Var for Editor Viewing
    {
        trDirection.eulerAngles = new Vector3(0, Rotation, 0);
        direction = trDirection.right;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, box.size);
        Gizmos.DrawWireSphere(particles.position, 1f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + direction * 20);
    }
}
