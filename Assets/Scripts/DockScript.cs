using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Switches between Land & Sea Modes
/// </summary>
public class DockScript : GameStage
{
    [SerializeField] GameObject DemoEndScreen;
    [SerializeField] Collider dockPArea; //Where player interacts with dock
    [SerializeField] Collider dockBArea; //Where boat interacts with dock
    [SerializeField] Transform playerPoint; //Player Spawn
    [SerializeField] Transform boatPoint; //Boat Spawn
    [SerializeField] Transform player; //PlayerObj
    [SerializeField] BoatController boat; //BoatObj
    [SerializeField] CloudFollower clouds;
    [SerializeField] GameUI_Controller gameUI;
    [SerializeField] bool inDock; //If in dock
    [SerializeField] bool canSwitch = true;
    [SerializeField] int requiredItems;
    [SerializeField] WorldStage dockStage;
    [SerializeField] WorldStage seaStage;
    [SerializeField] GameProgression gameState;
    [SerializeField] AudioClip dockEnter;
    [SerializeField] AudioClip dockExit;
    [SerializeField] AudioSource dockSfx;

    public int Password { get { return requiredItems; } }
    private void Start()
    {
        if (inDock)
        {
            clouds.isInBoat = true;
        }
    }
    public void DockExit() //When Exiting Dock
    {
        if (gameState.CheckBarrier(requiredItems) && canSwitch) //WILL AUTOMATE PLAYER SETUP LATER
        {
            dockSfx.PlayOneShot(dockExit);
            gameUI.UI_Fade(dockExit.length);
            canSwitch = false;
            StartCoroutine(DockExitTimer());
            //DemoEndScreen.SetActive(true);
        }
    }
    public void DockEnter(Transform aBoat) //When Entering Dock
    {
        if (!inDock && canSwitch)
        {
            dockSfx.PlayOneShot(dockEnter);
            gameUI.UI_Fade(dockEnter.length);
            canSwitch = false;
            StartCoroutine(DockEnterTimer(aBoat));
            InputController.Instance.Instruments.ForceState(false);
        }
    }
    void SwitchGamestate(bool toLand)
    {
        if (toLand)
        {
            gameState.Stage = dockStage;
        }
        else { gameState.Stage = seaStage; }
        switch (gameState.Stage)
        {
            case WorldStage.Island1:
                MusicComposer1000.Instance.StartStoryMusic(1);
                break;
            case WorldStage.Island2:
                MusicComposer1000.Instance.StartStoryMusic(2);
                break;
            case WorldStage.Island3:
                MusicComposer1000.Instance.StartStoryMusic(3);
                break;
            case WorldStage.MidSea:
                MusicComposer1000.Instance.StartPlucksMusic();
                break;
            case WorldStage.OpenSea:
                MusicComposer1000.Instance.StartPlucksMusic();
                break;
        }
    }
    IEnumerator SwitchTimer()
    {
        canSwitch = false;
        yield return new WaitForSeconds(1);
        canSwitch = true;
    }
    IEnumerator DockEnterTimer(Transform aBoat)
    {
        boat.EnableDisableBounce(false);
        yield return new WaitForSeconds(1);
        player.position = playerPoint.position;
        player.gameObject.SetActive(true);
        aBoat.position = boatPoint.position;
        aBoat.rotation = boatPoint.rotation;
        boat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
        boat.DisableBoat();
        inDock = true;
        clouds.isInBoat = true;
        SwitchGamestate(true);
        StartCoroutine(SwitchTimer());
        yield return new WaitForSeconds(0.5f);
        CameraScript.Instance.RotateCamera(playerPoint.localEulerAngles);
    }
    IEnumerator DockExitTimer()
    {
        yield return new WaitForSeconds(1);
        player.position = playerPoint.position;
        player.gameObject.SetActive(false);
        boat.EnableBoat();
        inDock = false;
        clouds.isInBoat = true;
        boat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        SwitchGamestate(false);
        StartCoroutine(SwitchTimer());
        if (gameState.previousStage == WorldStage.Island1) { TutorialScript.Instance.OpenTutorial(); }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(dockPArea.transform.position, dockPArea.GetComponent<BoxCollider>().size);
        Gizmos.DrawWireSphere(dockBArea.transform.position, dockBArea.GetComponent<SphereCollider>().radius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(playerPoint.position, playerPoint.forward);
    }
}
