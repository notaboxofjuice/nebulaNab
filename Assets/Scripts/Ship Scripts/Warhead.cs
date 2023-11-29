using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warhead : MonoBehaviour
{
    [SerializeField] float forceAcceleration = 5f;
    Rigidbody body;
    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Motor();
    }
    void Motor()
    {
        body.AddForce(forceAcceleration * transform.forward, ForceMode.Acceleration);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            //damage ship
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {
            transform.Rotate(0, 180, 0);
        }
    }
}
