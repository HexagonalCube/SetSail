using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockScript : MonoBehaviour
{
    [SerializeField] Collider dockPArea;
    [SerializeField] Collider dockBArea;
    [SerializeField] Transform playerPoint;
    [SerializeField] Transform boatPoint;
    [SerializeField] Transform player;
    [SerializeField] BoatController boat;
    bool inDock;
    public void DockExit()
    {
        player.position = playerPoint.position;
        player.gameObject.SetActive(false);
        boat.EnableBoat();
        inDock = false;
    }
    public void DockEnter(Transform aBoat)
    {
        if (!inDock)
        {
            player.gameObject.SetActive(true);
            aBoat.position = boatPoint.position;
            aBoat.rotation = boatPoint.rotation;
            boat.DisableBoat();
            inDock = true;
        }
    }
}
