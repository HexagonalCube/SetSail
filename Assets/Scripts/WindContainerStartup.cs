using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Initializes all wind areas as per settings
/// </summary>
public class WindContainerStartup : MonoBehaviour
{
    WindArea[] windAreas;
    // Start is called before the first frame update
    void Start()
    {
        windAreas = GetComponentsInChildren<WindArea>();
        for (int i = 0; i < windAreas.Length; i++)
        {
            windAreas[i].VisibilitySwitch(false);
        }
    }
}
