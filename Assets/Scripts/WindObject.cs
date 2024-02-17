using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindObject : MonoBehaviour
{
    public bool inWindZone = false;
    public GameObject windZone;

    Rigidbody rb;

    Vector3 windCurrent;
    float windSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inWindZone)
        {
            rb.AddForce(windCurrent * windSpeed);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "windArea")
        {
            windZone = col.gameObject;
            inWindZone = true;
            windCurrent = windZone.GetComponent<WindArea>().direction;
            windSpeed = windZone.GetComponent<WindArea>().strenght;
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "windArea")
        {
            windZone = col.gameObject;
            inWindZone = true;
            windCurrent = windZone.GetComponent<WindArea>().direction;
            windSpeed = windZone.GetComponent<WindArea>().strenght;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "windArea")
        {
            inWindZone = false;
        }
    }
}
