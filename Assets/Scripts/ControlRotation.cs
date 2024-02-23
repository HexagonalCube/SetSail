using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRotation : MonoBehaviour
{
    [SerializeField] bool swapKeys=false;
    [SerializeField] float maxRotation;
    [SerializeField] Animator animator;
    [SerializeField] float inputRotation;

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
                if (inputRotation > -1) { inputRotation -= 1 * Time.deltaTime; }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (inputRotation < 1) { inputRotation += 1 * Time.deltaTime; }
            }
            else
            {
                if (inputRotation > 0) { inputRotation -= 1 * Time.deltaTime; }
                else if (inputRotation < 0) { inputRotation += 1 * Time.deltaTime; }
            }
        }
    }
    private void FixedUpdate()
    {
        if (!swapKeys)
        {
            animator.SetFloat("xAxis", inputRotation);
            transform.Rotate(new Vector3(0, 45 * inputRotation, 0) * Time.deltaTime);
        } 
    }
}
