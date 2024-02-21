using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOverlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
    }
}
