using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BoatController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] BuoyancyObject buoy;
    [SerializeField] WindObject wind;
    [SerializeField] public ControlRotation rotateBoat;
    [SerializeField] public ControlRotation rotateSail;
    [SerializeField] CameraScript mainCamera;
    [SerializeField] bool startEnabled = false;
    [SerializeField] bool nearDock;
    [SerializeField] DockScript dock;
    [SerializeField] GameObject sailGeo;
    [SerializeField] SFXController boatSFX;
    [SerializeField] bool basicEnabled = true;
    [SerializeField] GameUI_Controller gameUI;
    public bool sailUp = false;
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
        rotateBoat = GetComponent<ControlRotation>();
    }
    private void OnValidate()
    {
        GetVariables();
    }
    public void EnableBoat()
    {
        gameUI.Interact(false);
        gameUI.scheduleFadeOut = true;
        wind.enabled = true;
        rotateBoat.rotEnabled = true;
        rotateSail.rotEnabled = true;
        mainCamera.SwitchCamera(true);
        basicEnabled = true;
        sailGeo.SetActive(true);
        sailUp = false;
        boatSFX.EnterBoat();
        LowerSail();
        boatSFX.sailRaised = false;
    }
    public void DisableBoat()
    {
        rb.velocity = Vector3.zero;
        wind.enabled = false;
        rotateBoat.rotEnabled = false;
        rotateSail.rotEnabled = false;
        mainCamera.SwitchCamera(false);
        basicEnabled = false;
        sailGeo.SetActive(false);
        boatSFX.ExitBoat();
        sailUp = true;
        boatSFX.sailRaised = true;
    }
    public void RaiseSail()
    {
        if (!sailUp & basicEnabled)
        {
            sailUp = true;
            wind.SwitchBoatStopped(sailUp);
            sailGeo.SetActive(false);
            boatSFX.sailRaised = true;
        }
    }
    public void LowerSail()
    {
        if (sailUp & basicEnabled)
        {
            sailUp = false;
            wind.SwitchBoatStopped(sailUp);
            sailGeo.SetActive(true);
            boatSFX.sailRaised = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DockEnter"))
        {
            //Trigger Visual Indication HERE (like button overlay or smth)
            if (basicEnabled) { gameUI.Interact(true); }
            //
            dock = other.GetComponentInParent<DockScript>();
            nearDock = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DockEnter"))
        {
            //Trigger Visual Indication HERE (like button overlay or smth)
            gameUI.Interact(false);
            //
            dock = null;
            nearDock = false;
        }
    }
    public void EnterDock()
    {
        if (nearDock && dock!=null)
        {
            gameUI.Interact(false);
            dock.DockEnter(transform);
            dock = null;
        }
    }
}
