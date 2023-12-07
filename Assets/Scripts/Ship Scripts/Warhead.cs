using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
        body.AddForce(forceAcceleration * transform.up, ForceMode.Force);
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collsion with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ship"))
        {
            collision.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {
            transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
}
