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
    bool inDock; //If in dock
    public void DockExit() //When Exiting Dock
    {
        player.position = playerPoint.position;
        player.gameObject.SetActive(false);
        boat.EnableBoat();
        inDock = false;
    }
    public void DockEnter(Transform aBoat) //When Entering Dock
    {
        if (!inDock)
        {
            player.position = playerPoint.position;
            player.gameObject.SetActive(true);
            aBoat.position = boatPoint.position;
            aBoat.rotation = boatPoint.rotation;
            boat.DisableBoat();
            inDock = true;
        }
    }
}
