using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDistance : MonoBehaviour
{
    [SerializeField] PlayerInteract pInt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            pInt.closeToItem = true;
            pInt.distanceToItem = Vector3.Distance(transform.position, other.transform.position);
            pInt.curItem = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.GetComponent<ItemScript>().HiglightObject(false);
            pInt.closeToItem = false;
            pInt.curItem = null;
        }
    }
}
