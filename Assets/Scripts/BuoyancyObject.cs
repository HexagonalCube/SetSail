using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Buoyancy physics on the Y axis
/// Dependent on Heightmaps for water elevation
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BuoyancyObject : MonoBehaviour
{
    public Transform[] floaters;

    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;

    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;

    public float floatingPower = 15f;

    [SerializeField] OceanManager oceanManager;

    [SerializeField] Rigidbody hull_Rb;

    [SerializeField] int floatersUnderwater;

    [SerializeField] bool underwater;

    void Start() //Get values
    {
        hull_Rb = GetComponent<Rigidbody>();
        oceanManager = FindObjectOfType<OceanManager>();
    }


    void FixedUpdate()
    {
        floatersUnderwater = 0;
        for (int i = 0; i < floaters.Length; i++) //Checks if floater is underwater
        {
            float diff = floaters[i].position.y - oceanManager.WaterHeightAtPosition(floaters[i].position); //Water Height to floater height difference

            if (diff < 0)
            {
                hull_Rb.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(diff), floaters[i].position, ForceMode.Force); //Add buoyancy force relative to difference
                floatersUnderwater += 1;
                if (!underwater)
                {
                    underwater = true;
                    SwitchState(true);
                }
            }
        }
        if (underwater && floatersUnderwater == 0) //If none underwater
        {
            underwater = false;
            SwitchState(false);
        }
    }

    void SwitchState(bool isUnderwater) //Switches the physics values between Water&Air
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
        Gizmos.color= Color.blue;
        Gizmos.DrawWireSphere(hull_Rb.centerOfMass + transform.position, 1f);
        //Debug.Log(hull_Rb.centerOfMass);
    }
}
