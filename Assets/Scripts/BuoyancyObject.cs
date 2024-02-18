using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyancyObject : MonoBehaviour
{
    public Transform[] floaters;

    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;

    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;

    public float floatingPower = 15f;

    OceanManager oceanManager;

    Rigidbody hull_Rb;

    int floatersUnderwater;

    bool underwater;

    void Start()
    {
        hull_Rb = GetComponent<Rigidbody>();
        oceanManager = FindObjectOfType<OceanManager>();
    }


    void FixedUpdate()
    {
        floatersUnderwater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float diff = floaters[i].position.y - oceanManager.WaterHeightAtPosition(floaters[i].position);

            if (diff < 0)
            {
                hull_Rb.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(diff), floaters[i].position, ForceMode.Force);
                floatersUnderwater += 1;
                if (!underwater)
                {
                    underwater = true;
                    SwitchState(true);
                }
            }
        }
        if (underwater && floatersUnderwater == 0)
        {
            underwater = false;
            SwitchState(false);
        }
    }

    void SwitchState(bool isUnderwater)
    {
        if(isUnderwater)
        {
            hull_Rb.drag = underWaterDrag;
            hull_Rb.angularDrag = underWaterAngularDrag;
        }
        else
        {
            hull_Rb.drag = airDrag;
            hull_Rb.angularDrag = airAngularDrag;
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < floaters.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(floaters[i].position, 0.2f);
        }
    }
}
