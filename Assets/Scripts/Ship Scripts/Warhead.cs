using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warhead : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    private void FixedUpdate()
    {
        
    }
    void Motor()
    {
        transform.Translate(transform.forward * movementSpeed * Time.fixedDeltaTime);
    }
}
