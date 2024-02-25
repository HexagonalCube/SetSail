using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public float speed = 10;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Move();
    }
    void Move()
    {
        rb.velocity = transform.forward * Input.GetAxis("Vertical") * speed + transform.right * Input.GetAxis("Horizontal") * speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DockExit"))
        {
            other.GetComponentInParent<DockScript>().DockExit();
            Debug.Log("ExitToBoat");
        }
    }
}
