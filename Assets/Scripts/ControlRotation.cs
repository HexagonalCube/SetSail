using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRotation : MonoBehaviour
{
    [SerializeField] bool swapKeys=false;
    [SerializeField] float maxRotation;

    // Update is called once per frame
    void Update()
    {
        float angle = transform.localRotation.y;
        if (swapKeys)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && angle > -maxRotation)
            {
                transform.Rotate(new Vector3(0, -50, 0) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow) && angle < maxRotation)
            {
                transform.Rotate(new Vector3(0, 50, 0) * Time.deltaTime);
            }
            //Debug.Log(angle);
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
