using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField] Transform rot;
    [SerializeField] Transform reference;
    [SerializeField] Vector3 axisOfRot;
    [SerializeField] Vector3 eulersOffset;

    private void Update()
    {
        float angle = Vector3.SignedAngle(new Vector3(reference.forward.x, 0,reference.forward.z), Vector3.forward,Vector3.up);
        rot.localEulerAngles = new Vector3((angle * axisOfRot.x) + eulersOffset.x, (angle * axisOfRot.y) + eulersOffset.y, (angle * axisOfRot.z) + eulersOffset.z);
    }
}
