using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Makes the ocean follow the player in the lowPoly grid
/// </summary>
public class OceanFollower : MonoBehaviour
{
    public Transform boat;
    public Transform player;

    [SerializeField] float gridSnap = 2;

    void Update() //Makes the object follow the player within the assigned grid
    {
        if (CameraScript.Instance.InBoat)
        {
            Vector3 boatPos = boat.position;
            boatPos = new Vector3(Mathf.FloorToInt(boatPos.x / gridSnap), 0, Mathf.FloorToInt(boatPos.z / gridSnap));
            boatPos *= gridSnap;
            //Debug.Log($"Calculated {boatPos} Real{boat.position}");

            transform.position = new Vector3(boatPos.x, transform.position.y, boatPos.z);
        }
        else
        {
            Vector3 pPos = player.position;
            pPos = new Vector3(Mathf.FloorToInt(pPos.x / gridSnap), 0, Mathf.FloorToInt(pPos.z / gridSnap));
            pPos *= gridSnap;

            transform.position = new Vector3(pPos.x, transform.position.y, pPos.z);
        }
    }
}
