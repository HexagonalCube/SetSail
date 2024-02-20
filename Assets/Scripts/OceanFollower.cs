using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanFollower : MonoBehaviour
{
    public Transform boat;

    [SerializeField] float gridSnap = 2;

    void Update() //Mantem o mar junto ao barco dentro de uma grid
    {
        Vector3 boatPos = boat.position;
        boatPos = new Vector3 (Mathf.FloorToInt(boatPos.x / gridSnap), 0, Mathf.FloorToInt(boatPos.z / gridSnap));
        boatPos = boatPos * gridSnap;
        //Debug.Log($"Calculated {boatPos} Real{boat.position}");

        transform.position = new Vector3(boatPos.x,transform.position.y,boatPos.z);
    }
}
