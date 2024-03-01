using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Camera overlay Settings
/// Boat overscan
/// </summary>
public class CameraOverlay : MonoBehaviour
{
    void Start()
    {
        transform.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
    }
}
