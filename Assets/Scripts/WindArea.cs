using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    public float Rotation;
    public Transform trDirection;
    public Vector3 direction;
    public Transform particles;

    [SerializeField] BoxCollider box;
    private void OnValidate()
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
