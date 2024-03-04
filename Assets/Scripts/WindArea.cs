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
    public ParticleSystem[] particles; //Particles object

    [SerializeField] BoxCollider box; //Effective Area
    private void OnValidate() //Always set Var for Editor Viewing
    {
        trDirection.eulerAngles = new Vector3(0, Rotation, 0);
        direction = trDirection.right;
    }
    public void VisibilitySwitch(bool isVisible) //Switches particles emmission
    {
        foreach (var particle in particles)
        {
            var emmision = particle.emission;
            emmision.enabled = isVisible;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, box.size);
        Gizmos.DrawWireSphere(particles[0].transform.position, 1f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + direction * 20);
    }
}
