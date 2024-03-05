using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Follow boat position, but remain at current Y value
/// </summary>
public class CloudFollower : MonoBehaviour
{
    [SerializeField] Transform boat;
    [SerializeField] Transform player;
    public bool isInBoat = true;

    void Update()
    {
        switch (isInBoat)
        {
            case false:
                transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
                break;
            case true:
                transform.position = new Vector3(boat.position.x, transform.position.y, boat.position.z);
                break;
        }
    }
}
