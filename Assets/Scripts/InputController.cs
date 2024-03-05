using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] BoatController bController;
    [SerializeField] PlayerController pController;
    [SerializeField] PlayerInteract pInteract;

    [SerializeField] KeyCode interact = KeyCode.E;
    [SerializeField] KeyCode sailPositive = KeyCode.W;
    [SerializeField] KeyCode sailNegative = KeyCode.S;
    [SerializeField] KeyCode raiseLowerSail = KeyCode.Space;
    [SerializeField] KeyCode rudderPositive = KeyCode.A;
    [SerializeField] KeyCode rudderNegative = KeyCode.D;

    void Update()
    {
        //Interactions
        if (Input.GetKeyDown(interact))
        {
            bController.EnterDock();
            pController.ExitDock();
            pInteract.Interact();
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
            if (bController.sailUp)
            {
                bController.LowerSail();
            }
            else
            {
                bController.RaiseSail();
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
