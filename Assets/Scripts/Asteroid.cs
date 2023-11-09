using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float travelSpeed;
    [Tooltip("The maximum size the asteroid reaches at the midpoint of it's travel.")]
    [SerializeField] float maxSize;
    [Tooltip("Maximum amount of time the object is allowed to persist before it destroys itself.")]
    [SerializeField] float killTime;
    ObjectSpawner spawner;

    private void Awake()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<ObjectSpawner>();
    }
    void Start()
    {
        Destroy(gameObject, killTime);
    }
    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (travelSpeed * Time.deltaTime), transform.position.z);
    }
    private void OnDestroy()
    {
        spawner.obstacleCount--;
    }
}
