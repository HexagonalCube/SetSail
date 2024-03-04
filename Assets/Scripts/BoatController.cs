using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] BuoyancyObject buoy;
    [SerializeField] WindObject wind;
    [SerializeField] ControlRotation rotate;
    [SerializeField] CameraScript mainCamera;
    [SerializeField] bool startEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        GetVariables();
        if (startEnabled)
        {
            EnableBoat();
        }
        else
        {
            DisableBoat();
        }
    }
    void GetVariables()
    {
        rb = GetComponent<Rigidbody>();
        buoy = GetComponent<BuoyancyObject>();
        wind = GetComponent<WindObject>();
        rotate = GetComponent<ControlRotation>();
    }
    private void OnValidate()
    {
        GetVariables();
    }
    public void EnableBoat()
    {
        wind.enabled = true;
        rotate.enabled = true;
        mainCamera.SwitchCamera(true);
    }
    public void DisableBoat()
    {
        rb.velocity = Vector3.zero;
        wind.enabled = false;
        rotate.enabled = false;
        mainCamera.SwitchCamera(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DockEnter"))
        {
            //Trigger Visual Indication HERE (like button overlay or smth)
            //
            //
            if (Input.GetKey(KeyCode.E))
            {
                //Trigger Event
                other.GetComponentInParent<DockScript>().DockEnter(transform);
                Debug.Log("EnterDock");
            }
        }
    }
}
