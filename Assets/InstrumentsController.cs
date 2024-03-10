using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentsController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] bool startRaised = false;
    bool raised;
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (startRaised)
        {
            animator.Play("IdleRaisedCompass");
        }
        else
        {
            animator.Play("IdleLoweredCompass");
        }
    }
    public void RaiseLowerCompass()
    {
        if (raised)
        {
            raised = false;
            animator.Play("LowerCompass");
        }
        else
        {
            raised = true;
            animator.Play("RaiseCompass");
        }
    }
}
