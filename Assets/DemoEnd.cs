using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEnd : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.Log("A");
        Application.Quit();
    }
}
