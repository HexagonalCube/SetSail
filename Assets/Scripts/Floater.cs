using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] Collider col;
    public bool underwater;
    public float depth = 0;
    public float height = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sea"))
        {
            underwater = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sea"))
        {
            underwater = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sea"))
        {
            underwater = false;
        }
    }

}
