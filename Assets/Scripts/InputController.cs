using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] BoatController bController;
    [SerializeField] PlayerController pController;
    [SerializeField] PlayerInteract pInteract;
    [SerializeField] InstrumentsController pInstruments;

    [SerializeField] KeyCode interact = KeyCode.E;
    [SerializeField] KeyCode raiseLowerCompass = KeyCode.C;
    [SerializeField] KeyCode sailPositive = KeyCode.W;
    [SerializeField] KeyCode sailNegative = KeyCode.S;
    [SerializeField] KeyCode raiseLowerSail = KeyCode.Space;
    [SerializeField] KeyCode rudderPositive = KeyCode.A;
    [SerializeField] KeyCode rudderNegative = KeyCode.D;
    [SerializeField] KeyCode diaryKey = KeyCode.Tab;
    [SerializeField] KeyCode pauseKey = KeyCode.Escape;
    #region External
    public KeyCode PInt { get { return interact; } }
    public KeyCode SailP { get { return sailPositive;} }
    public KeyCode SailN { get { return sailNegative; } }
    public KeyCode RudderP {  get { return rudderPositive; } }
    public KeyCode RudderN { get { return rudderNegative; } }
    public KeyCode SailRL { get { return raiseLowerSail; } }
    public KeyCode Pause { get { return pauseKey; } }
    public KeyCode Diary { get { return diaryKey; } }
    #endregion

    void Update()
    {
        //Interactions
        if (Input.GetKeyDown(interact))
        {
            bController.EnterDock();
            pController.ExitDock();
            pInteract.Interact();
        }
        if (Input.GetKeyDown(raiseLowerCompass))
        {
            pInstruments.RaiseLowerCompass();
        }
        //BoatControls
        if (Input.GetKey(sailNegative))
        {
            bController.rotateSail.TurnSailRight();
        }
        if (Input.GetKey(sailPositive))
        {
            bController.rotateSail.TurnSailLeft();
        }
        if (Input.GetKeyDown(raiseLowerSail))
        {
            if (bController.sailStowed)
            {
                bController.ReleaseSail();
            }
            else
            {
                bController.StowSail();
            }
        }
        if (Input.GetKey(rudderNegative))
        {
            bController.rotateBoat.TurnRudderRight();
        }
        if (Input.GetKey(rudderPositive))
        {
            bController.rotateBoat.TurnRudderLeft();
        }
    }
}
