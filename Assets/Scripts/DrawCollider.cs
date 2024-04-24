using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawCollider : MonoBehaviour
{
    enum ColliderType { Sphere, Box, Mesh }
    SphereCollider sc;
    BoxCollider bc;
    MeshCollider mc;
    [SerializeField] Mesh mcMesh;
    [SerializeField] ColliderType type;
    [ColorUsage(false)] public Color color;

    private void OnValidate()
    {
        switch (type)
        {
            case ColliderType.Sphere:
                sc = GetComponent<SphereCollider>();
                break;
            case ColliderType.Box:
                bc = GetComponent<BoxCollider>();
                break;
            case ColliderType.Mesh:
                mc = GetComponent<MeshCollider>();
                break;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(color.r, color.g, color.b);
        switch (type)
        {
            case ColliderType.Sphere:
                Gizmos.DrawWireSphere(transform.position + sc.center, sc.radius);
                break;
            case ColliderType.Box:
                if (mcMesh == null)
                {
                    Gizmos.DrawWireCube(transform.position, bc.size);
                }
                else
                {
                    Gizmos.DrawWireMesh(mcMesh, transform.position, transform.rotation, bc.size + (transform.localScale - Vector3.one));
                }
                break;
            case ColliderType.Mesh:
                Gizmos.DrawWireMesh(mcMesh, -1, transform.position, transform.rotation);
                break;
        }
    }
}
