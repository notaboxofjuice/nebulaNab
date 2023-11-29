using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warhead : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    private void FixedUpdate()
    {
        Motor();
    }
    void Motor()
    {
        transform.Translate(transform.forward * movementSpeed * Time.fixedDeltaTime);
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
