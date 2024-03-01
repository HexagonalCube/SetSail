using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Player-Item Interactions
/// Fov & view based
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    public bool closeToItem;
    public float distanceToItem;
    public Transform curItem;
    [SerializeField] float maxDist;
    [SerializeField] float minAngle;
    [SerializeField] bool isInView;

    public void Interact() //Get item interaction
    {
        if (isInView)
        {
            curItem.GetComponent<ItemScript>().Interact();
        }
    }
    bool inview(Vector3 campos, Vector3 camdir, Vector3 bodypos ) //Checks if in view
    {
        //Get the vector to body
        Vector3 tobody = bodypos - campos;
        #region Unused
        //Vector3 tobody2d = tobody;
        //tobody2d.z = 0;
        //Vector3 camdir2d = camdir;
        //camdir2d.z = 0;
        //tobody2d = tobody2d.normalized;
        //camdir2d = camdir2d.normalized;
        //float dp = Vector3.Dot(tobody2d, camdir2d);
        #endregion
        //Get the angle to body
        float tiltcam = Mathf.Asin(camdir.z);
        tobody = tobody.normalized;
        float tilttobody = Mathf.Asin(tobody.z);
        //Get total difference
        float angleDiff = Mathf.Abs(tilttobody - tiltcam);
        //If whitin diff, then return true
        if (angleDiff > minAngle)
        {
            return false;
        }
        else { return true; }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (curItem != null) { Gizmos.DrawLine(transform.position, curItem.position); }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
    }

    //temporary
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        if (curItem != null) 
        {
            isInView = inview(transform.position ,transform.forward ,curItem.position );
            curItem.GetComponent<ItemScript>().HiglightObject(isInView);
        }
    }
}
