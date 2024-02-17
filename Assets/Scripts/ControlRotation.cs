using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRotation : MonoBehaviour
{
    [SerializeField] bool swapKeys=false;

    // Update is called once per frame
    void Update()
    {
        if (swapKeys)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(new Vector3(0, -50, 0) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(0, 50, 0) * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(new Vector3(0, -50, 0) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(new Vector3(0, 50, 0) * Time.deltaTime);
            }
        }
    }
}
