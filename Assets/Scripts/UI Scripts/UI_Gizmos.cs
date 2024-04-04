using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Vai sobrepor gizmos no ui para alinhar
/// </summary>
public class UI_Gizmos : MonoBehaviour
{
    [SerializeField] float tamanhoDoCirculo = 1000f;
    [SerializeField] float tamanhoDoCubo = 1000f;
    [SerializeField] Color corCirculo = Color.white;
    [SerializeField] Color corCubo = Color.white;
    private void OnDrawGizmos()
    {
        Gizmos.color = corCirculo;
        Gizmos.DrawWireSphere(transform.position, tamanhoDoCirculo);
        Gizmos.color = corCubo;
        Gizmos.DrawWireCube(transform.position, Vector3.one * tamanhoDoCubo);
    }
}
