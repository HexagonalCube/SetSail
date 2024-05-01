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
    [SerializeField] Animator sailAnim;
    [SerializeField] GameObject sailGeo;
    [SerializeField] SFXController boatSFX;
    [SerializeField] bool basicEnabled = true;
    [SerializeField] GameUI_Controller gameUI;
    public bool sailStowed = false;
    // Start is called before the first frame update
    void Start()
    {
        GetVariables();
        if (startEnabled)
        {
            sailStowed = false;
            sailAnim.Play("RaisedSail");
            EnableBoat();
        }
        else
        {
            sailStowed = true;
            sailAnim.Play("LoweredSail");
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
        wind.SwitchBoatStopped(true);
        rotateBoat.rotEnabled = true;
        rotateSail.rotEnabled = true;
        mainCamera.SwitchCamera(true);
        basicEnabled = true;
        //sailStowed = false;
        boatSFX.EnterBoat();
        StowSail();
        boatSFX.sailRaised = true;
    }
    public void DisableBoat()
    {
        rb.velocity = Vector3.zero;
        wind.enabled = false;
        wind.SwitchBoatStopped(true);
        rotateBoat.rotEnabled = false;
        rotateSail.rotEnabled = false;
        rotateSail.transform.localEulerAngles = Vector3.zero;
        mainCamera.SwitchCamera(false);
        boatSFX.ExitBoat();
        //sailStowed = true;
        StowSail();
        boatSFX.sailRaised = true;
        basicEnabled = false;
    }
    public void StowSail()
    {
        if (!sailStowed & basicEnabled)
        {
            sailStowed = true;
            wind.SwitchBoatStopped(sailStowed);
            boatSFX.sailRaised = true;
            sailAnim.Play("LoweredSail");
            SailSfx.Instance.Sailing = false;
        }
    }
    public void ReleaseSail()
    {
        if (sailStowed & basicEnabled)
        {
            sailStowed = false;
            wind.SwitchBoatStopped(sailStowed);
            boatSFX.sailRaised = false;
            sailAnim.Play("RaisedSail");
            SailSfx.Instance.Sailing = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DockEnter"))
        {
            //Trigger Visual Indication HERE (like button overlay or smth)
            if (basicEnabled) { gameUI.Interact(true, "Atracar (E)"); }
            //
            dock = other.GetComponentInParent<DockScript>();
            nearDock = true;
            if (GameProgression.Instance.previousStage == GameStage.WorldStage.Intro) { TutorialScript.Instance.HideShowTutorial(true); }
        }
        if (other.CompareTag("LastIsland"))
        {
            IslandsDistanceController.Instance.SetLastIsland = true;
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
        if (other.CompareTag("LastIsland"))
        {
            IslandsDistanceController.Instance.SetLastIsland = false;
        }
    }
    public void EnterDock()
    {
        if (nearDock && dock!=null)
        {
            gameUI.CanInteract = true;
            gameUI.Interact(false);
            dock.DockEnter(transform);
            dock = null;
        }
    }
}
