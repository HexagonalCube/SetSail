using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    public Vector3 direction;

    [SerializeField] BoxCollider box;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, box.size);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + direction * 10);
    }
}
