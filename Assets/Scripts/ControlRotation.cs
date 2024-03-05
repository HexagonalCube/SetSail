using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Placeholder Mechanics for boat rotation controls
/// </summary>
public class ControlRotation : MonoBehaviour
{
    [SerializeField] bool swapKeys=false;
    [SerializeField] float maxRotation;
    [SerializeField] Animator animator;
    [SerializeField] float inputRotation;
    [SerializeField] public bool rotEnabled = true;
    float angle;


    void Update()
    {
        angle = transform.localRotation.y;
        if (!Input.anyKey)
        {
            if (inputRotation > 0) { inputRotation -= 1 * Time.deltaTime; }
            else if (inputRotation < 0) { inputRotation += 1 * Time.deltaTime; }
        }
    }
    public void TurnSailLeft()
    {
        if (angle > -maxRotation && swapKeys && rotEnabled)
        {
            //Debug.Log("Turning");
            transform.Rotate(new Vector3(0, -50, 0) * Time.deltaTime);
        }
    }
    public void TurnSailRight()
    {
        if (angle < maxRotation && swapKeys && rotEnabled)
        {
            //Debug.Log("Turning");
            transform.Rotate(new Vector3(0, 50, 0) * Time.deltaTime);
        }
    }
    public void TurnRudderLeft()
    {
        if (inputRotation > -1 && !swapKeys && rotEnabled)
        {
            inputRotation -= 1 * Time.deltaTime;
        }
    }
    public void TurnRudderRight()
    {
        if (inputRotation < 1 && !swapKeys && rotEnabled)
        {
            inputRotation += 1 * Time.deltaTime;
        }
    }
    private void FixedUpdate() //Animator values update
    {
        if (!swapKeys)
        {
            animator.SetFloat("xAxis", inputRotation);
            transform.Rotate(new Vector3(0, 45 * inputRotation, 0) * Time.deltaTime);
        } 
    }
}
