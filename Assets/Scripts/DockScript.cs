using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Switches between Land & Sea Modes
/// </summary>
public class DockScript : MonoBehaviour
{
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
    private void Start()
    {
        if (inDock)
        {
            clouds.isInBoat = true;
        }
    }
    public void DockExit() //When Exiting Dock
    {
        if (/*inDock && */canSwitch) //WILL AUTOMATE PLAYER SETUP LATER
        {
            gameUI.UI_Fade(2f);
            canSwitch = false;
            StartCoroutine(DockExitTimer());
        }
    }
    public void DockEnter(Transform aBoat) //When Entering Dock
    {
        if (!inDock && canSwitch)
        {
            gameUI.UI_Fade(2f);
            canSwitch = false;
            StartCoroutine(DockEnterTimer(aBoat));
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
        yield return new WaitForSeconds(1);
        player.position = playerPoint.position;
        player.gameObject.SetActive(true);
        aBoat.position = boatPoint.position;
        aBoat.rotation = boatPoint.rotation;
        boat.DisableBoat();
        inDock = true;
        clouds.isInBoat = true;
        StartCoroutine(SwitchTimer());
    }
    IEnumerator DockExitTimer()
    {
        yield return new WaitForSeconds(1);
        player.position = playerPoint.position;
        player.gameObject.SetActive(false);
        boat.EnableBoat();
        inDock = false;
        clouds.isInBoat = true;
        StartCoroutine(SwitchTimer());
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(dockPArea.transform.position, dockPArea.GetComponent<BoxCollider>().size);
        Gizmos.DrawWireSphere(dockBArea.transform.position, dockBArea.GetComponent<SphereCollider>().radius);
    }
}
