using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Gives Wind forces when inside wind zones
/// </summary>
public class WindObject : MonoBehaviour
{
    public bool inWindZone = false; //If in windzone
    public GameObject windZone; //AOE

    public Rigidbody rb; //Boat
    public Transform sail; //Sail
    public Transform windIndicator; //WindFlag
    public Material flagMat; //FlagColor

    Vector3 windCurrent; //Wind Dir
    Vector3 sailDirection; //Sail Dir

    [SerializeField] float angleDiffR;
    [SerializeField] float angleDiffL;
    [SerializeField] float dirDiff;
    [SerializeField] float speedCur;

    public float angleMin; //MinAngle for wind effectiveness
    public float baseSpeed; //Base speed without wind
    public float minSpeed; //Speed against wind
    public float maxSpeed; //Speed with wind

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Update Variables
        sailDirection = sail.right;
        angleDiffR = Vector3.Angle(windCurrent, sailDirection);
        angleDiffL = Vector3.Angle(windCurrent, -sailDirection);
        dirDiff = Vector3.Angle(windCurrent, transform.forward);
        speedCur = rb.velocity.magnitude;
        if (!inWindZone)
        {
            windCurrent = Vector3.zero;
        }
    }
    void FixedUpdate()
    {
        if (inWindZone)//When effected by winds
        {
            windIndicator.forward = windCurrent;
            windIndicator.localEulerAngles = windIndicator.localEulerAngles - new Vector3(windIndicator.localEulerAngles.x, 0, windIndicator.localEulerAngles.z);
            if ((angleDiffR < angleMin || angleDiffL < angleMin) && dirDiff < 100)
            {
                //Maximum Force
                rb.AddForce(rb.transform.forward * maxSpeed);
                flagMat.color = Color.green;
            }
            else if ((angleDiffR < angleMin || angleDiffL < angleMin) && dirDiff > 100)
            {
                //Minimum Force
                rb.AddForce(rb.transform.forward * (minSpeed));
                flagMat.color = Color.red;
            }
            else
            {
                //Paralel to Wind force
                rb.AddForce(rb.transform.forward * (minSpeed * 3));
                flagMat.color = Color.cyan;
            }
        }
        else
        {
            //Normal Force
            windIndicator.localEulerAngles = Vector3.zero;
            rb.AddForce(rb.transform.forward * baseSpeed);
            flagMat.color = Color.yellow;
        }
    }
    private void OnTriggerEnter(Collider col)//Enter Wind
    {
        if(col.gameObject.tag == "windArea")
        {
            windZone = col.gameObject;
            inWindZone = true;
            windCurrent = windZone.GetComponent<WindArea>().direction;
        }
    }
    private void OnTriggerStay(Collider col)//In Wind
    {
        if (col.gameObject.tag == "windArea")
        {
            windZone = col.gameObject;
            inWindZone = true;
            windCurrent = windZone.GetComponent<WindArea>().direction;
        }
    }
    private void OnTriggerExit(Collider col)//Exit Wind
    {
        if (col.gameObject.tag == "windArea")
        {
            inWindZone = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(sail.position, sail.position + windCurrent * 10);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(sail.position, sail.position + sailDirection * 10);
        Gizmos.DrawLine(sail.position, sail.position + -sailDirection * 10);
    }
}
