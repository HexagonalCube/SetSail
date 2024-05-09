using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebound : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform tip;
    [SerializeField] float power = 1f;
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip clip;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BumpBarriers"))
        {
            Impact(collision.transform.forward);
        }
    }
    void Impact(Vector3 impactFoward)
    {
        rb.AddForceAtPosition(impactFoward * power, tip.position, ForceMode.Impulse);
        src.pitch = Random.Range(0.9f, 1.1f);
        src.PlayOneShot(clip);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(tip.position , 0.5f);
    }
}
