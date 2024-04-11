using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;
    [SerializeField] BoatController bController;
    [SerializeField] PlayerController pController;
    [SerializeField] PlayerInteract pInteract;
    [SerializeField] InstrumentsController pInstruments;
    [SerializeField] GameUI_Controller gameUI;
    [Space]
    [SerializeField] KeyCode interact = KeyCode.E;
    [SerializeField] KeyCode raiseLowerCompass = KeyCode.C;
    [SerializeField] KeyCode sailPositive = KeyCode.W;
    [SerializeField] KeyCode sailNegative = KeyCode.S;
    [SerializeField] KeyCode raiseLowerSail = KeyCode.Space;
    [SerializeField] KeyCode rudderPositive = KeyCode.A;
    [SerializeField] KeyCode rudderNegative = KeyCode.D;
    [SerializeField] KeyCode diaryKey = KeyCode.Tab;
    [SerializeField] KeyCode pauseKey = KeyCode.Escape;
    [Space]
    [SerializeField] bool inputEnabled = true;
    int tutorialStage;
    #region External
    public bool EnableInput { get { return inputEnabled; } set { inputEnabled = value; } }
    public KeyCode PInt { get { return interact; } }
    public KeyCode SailP { get { return sailPositive;} }
    public KeyCode SailN { get { return sailNegative; } }
    public KeyCode RudderP {  get { return rudderPositive; } }
    public KeyCode RudderN { get { return rudderNegative; } }
    public KeyCode SailRL { get { return raiseLowerSail; } }
    public KeyCode Pause { get { return pauseKey; } }
    public KeyCode Diary { get { return diaryKey; } }
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }
    private void Update()
    {
        tutorialStage = TutorialScript.Instance.TutProgress;
        if (inputEnabled)
        {
            InputUpdate();
        }
    }
    void InputUpdate()
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
        if (Input.GetKey(sailNegative) && tutorialStage > 2)
        {
            bController.rotateSail.TurnSailRight();
        }
        if (Input.GetKey(sailPositive) && tutorialStage > 2)
        {
            bController.rotateSail.TurnSailLeft();
        }
        if (Input.GetKeyDown(raiseLowerSail) && tutorialStage > 0)
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
        if (Input.GetKey(rudderNegative) && tutorialStage > 1)
        {
            bController.rotateBoat.TurnRudderRight();
        }
        if (Input.GetKey(rudderPositive) && tutorialStage > 1)
        {
            bController.rotateBoat.TurnRudderLeft();
        }
        if (Input.GetKeyDown(diaryKey))
        {
            if (gameUI.IsPaused)
            {
                gameUI.ClosePause();
            }
            else
            {
                gameUI.OpenStory();
            }
        }
        if (Input.GetKeyDown(pauseKey))
        {

            if (gameUI.IsPaused)
            {
                gameUI.ClosePause();
            }
            else
            {
                gameUI.OpenPause();
            }
        }
    }
}
