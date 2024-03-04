using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///Activates WindAreas based on distance and total objects
/// </summary>
public class WindRangeLimiter : MonoBehaviour
{
    [SerializeField] float drawRange;
    [SerializeField] int maxWindDraw;
    [SerializeField] Collider[] objs;
    [SerializeField] WindArea[] windAreas;
    [SerializeField] LayerMask windLayer;

    void FixedUpdate()
    {
        UpdateWindNearby();
    }
    void UpdateWindNearby()
    {
        objs = new Collider[200];
        windAreas = new WindArea[maxWindDraw];
        Physics.OverlapSphereNonAlloc(transform.position,drawRange, objs, windLayer);
        int i = 0;
        foreach (Collider obj in objs)
        {
            
            if (obj != null)
            {
                if (obj.CompareTag("windArea"))
                {
                    windAreas[i] = obj.GetComponent<WindArea>();
                    if (Vector3.Distance(obj.transform.position, transform.position) < drawRange - 0.1f)
                    {
                        windAreas[i].VisibilitySwitch(true);
                        i++;
                    }
                    else
                    {
                        windAreas[i].VisibilitySwitch(false);
                        windAreas[i] = null;
                        i++;
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, drawRange);
    }
}
