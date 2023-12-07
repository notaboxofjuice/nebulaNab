using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float travelSpeed;
    float growthSpeed;
    [Tooltip("The maximum size the asteroid reaches at the midpoint of it's travel.")]
    [SerializeField] float maxSize;
    [Tooltip("Maximum amount of time the object is allowed to persist before it destroys itself.")]
    [SerializeField] float killTime;
    ObjectSpawner spawner;

    private void Awake()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<ObjectSpawner>();
        growthSpeed = killTime * (Mathf.PI/180);
    }
    void Start()
    {
        //StartCoroutine(Grow());
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
        // PLAY EXPLODEY ANIMATION YEFERSON PLEASE FOR THE LOVE OF GOD DO SOMETHING
    }
    /*IEnumerator Grow()
    {
        float lerpPos = 0;
        float timer = 0;
        while(timer < killTime)
        {
            lerpPos = Mathf.PingPong(timer/(killTime/2), 1);
            transform.localScale = Vector3.one * Mathf.Lerp(0f, maxSize, lerpPos);
            timer += Time.deltaTime;
            yield return null;
        }
    }*/
}
