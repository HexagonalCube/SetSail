using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCollider : MonoBehaviour
{
    MeshCollider col;
    Mesh mesh;

    private void OnDrawGizmos()
    {
        if (col == null) col = GetComponent<MeshCollider>();
        if (mesh == null) mesh = GetComponent<MeshFilter>().sharedMesh;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireMesh(mesh,0,col.transform.position,col.transform.rotation,col.transform.lossyScale);
    }
}
