using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Finicky system but works, Moves the player using a character controller so slopes and stair are a non-issue
/// Gravity applied manually
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController cc;

    public float speed = 10;
    public float jmpHeight;
    public float gravity = -9.81f;

    public Vector3 pVelocity;
    public bool grounded;

    [SerializeField] bool nearDock;
    [SerializeField] DockScript dock;
    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Move();
    }
    void Move()
    {
        grounded = cc.isGrounded;
        if (grounded && pVelocity.y < 0)
        {
            pVelocity.y = 0f;
        }

        Vector3 move = transform.forward * Input.GetAxis("Vertical") * speed + transform.right * Input.GetAxis("Horizontal") * speed;
        cc.Move(move * Time.deltaTime * speed);

        //if (move != Vector3.zero)
        //{
        //    gameObject.transform.forward = move;
        //}

        // Changes the height position of the player..
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            pVelocity.y += Mathf.Sqrt(jmpHeight * -3.0f * gravity);
        }

        pVelocity.y += gravity * Time.deltaTime;
        cc.Move(pVelocity * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DockExit"))
        {
            //Trigger Visual Indication HERE (like button overlay or smth)
            //
            //
            dock = other.GetComponentInParent<DockScript>();
            nearDock = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DockExit"))
        {
            //Trigger Visual Indication HERE (like button overlay or smth)
            //
            //
            dock = null;
            nearDock = false;
        }
    }
    public void ExitDock()
    {
        if (nearDock && dock != null)
        {
            dock.DockExit();
            dock = null;
        }
    }
}
