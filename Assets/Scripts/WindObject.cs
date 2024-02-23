using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindObject : MonoBehaviour
{
    public bool inWindZone = false;
    public GameObject windZone;

    public Rigidbody rb; //Barco
    public Transform sail; //Vela

    Vector3 windCurrent; //Direcao do vento
    Vector3 sailDirection; //Direcao da vela

    [SerializeField] float angleDiffR;
    [SerializeField] float angleDiffL;
    [SerializeField] float dirDiff;
    [SerializeField] float speedCur;

    public float angleMin; //Minimo de angulo para vento tomar efeito
    public float baseSpeed; //Velocidade normal sem vento
    public float minSpeed; //Velocidade contra o vento
    public float maxSpeed; //velocidade junto ao vento

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
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
        if (inWindZone)//Quando afetado por ventos
        {
            if ((angleDiffR < angleMin || angleDiffL < angleMin) && dirDiff < 90)
            {
                //Forca Maxima
                rb.AddForce(rb.transform.forward * maxSpeed);
            }
            else if ((angleDiffR < angleMin || angleDiffL < angleMin) && dirDiff > 90)
            {
                //Forca Minima
                rb.AddForce(rb.transform.forward * (minSpeed));
            }
            else
            {
                //Forca Contra Vento
                rb.AddForce(rb.transform.forward * (minSpeed * 3));
            }
        }
        else
        {
            //Forca normal
            rb.AddForce(rb.transform.forward * baseSpeed);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "windArea")
        {
            windZone = col.gameObject;
            inWindZone = true;
            windCurrent = windZone.GetComponent<WindArea>().direction;
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "windArea")
        {
            windZone = col.gameObject;
            inWindZone = true;
            windCurrent = windZone.GetComponent<WindArea>().direction;
        }
    }
    private void OnTriggerExit(Collider col)
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
