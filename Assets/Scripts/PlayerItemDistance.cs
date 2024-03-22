using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDistance : MonoBehaviour
{
    [SerializeField] PlayerInteract pInt;
    ItemScript item;
    bool near;
    private void Update()
    {
        if (item != null)
        {
            item.HighlightObjectSimple(near);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            item = other.GetComponent<ItemScript>();
            near = true;
            pInt.closeToItem = true;
            pInt.distanceToItem = Vector3.Distance(transform.position, other.transform.position);
            pInt.curItem = other.transform;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            item = other.GetComponent<ItemScript>();
            near = true;
            pInt.closeToItem = true;
            pInt.distanceToItem = Vector3.Distance(transform.position, other.transform.position);
            pInt.curItem = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            near = false;
            StartCoroutine(SetNullAfterSeconds(0.2f));
        }
    }
    private IEnumerator SetNullAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        pInt.closeToItem = false;
        pInt.curItem = null;
    }
}
