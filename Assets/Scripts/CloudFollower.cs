using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Follow boat position, but remain at current Y value
/// </summary>
public class CloudFollower : MonoBehaviour
{
    public Transform boat;

    void Update()
    {
        transform.position = new Vector3(boat.position.x, transform.position.y, boat.position.z);
    }
}
