using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField] Transform rot;
    [SerializeField] Transform reference;

    private void Update()
    {
        float angle = Vector3.SignedAngle(new Vector3(reference.forward.x, 0,reference.forward.z), Vector3.forward,Vector3.up);
        rot.localEulerAngles = new Vector3(rot.eulerAngles.x, angle, rot.eulerAngles.z);
    }
}
