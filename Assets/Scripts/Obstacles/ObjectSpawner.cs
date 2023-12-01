using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class ObjectSpawner : MonoBehaviour
{
    #region Singleton
    private static ObjectSpawner instance;
    public static ObjectSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("ObjectSpawner");
                gameObject.AddComponent<ObjectSpawner>();
                Debug.Log("Created New Spawner");
            }
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    #endregion
    [SerializeField] float spawnRangeX;
    [SerializeField] float spawnRangeZ;
    [SerializeField] int maxJuiceObjects;
    [SerializeField] int maxObstacles;
    [SerializeField] float maxSpawnTime;
    [SerializeField] float minSpawnTime;
    [Tooltip("The Y coordinate with which to spawn asteroids")]
    [SerializeField] float asteroidYSpawnCoord;
    [Tooltip("Radial distance to check around a spawn point to avoid spawning on top of another object")]
    [SerializeField] float spawnRadius;
    [HideInInspector] public  int juiceObjectCount;
    [HideInInspector] public  int obstacleCount;
    bool objectNearby;
    [Tooltip("The objects to spawn that will be collected for juice")]
    [SerializeField] GameObject[] juiceObjects;
    [Tooltip("Asteroid obstacle objects to spawn")]
    [SerializeField] GameObject[] asteroids;

    [SerializeField] LayerMask spawnables;

    // Start is called before the first frame update
    void Start()
    {
        spawnRangeX = Mathf.Abs(spawnRangeX);
        spawnRangeZ = Mathf.Abs(spawnRangeZ);
        maxJuiceObjects = Mathf.Max(maxJuiceObjects);
        maxObstacles = Mathf.Max(maxObstacles);
        maxSpawnTime = Mathf.Max(maxSpawnTime);
        minSpawnTime = Mathf.Min(minSpawnTime);
        juiceObjectCount = 0;
        obstacleCount = 0;
        StartCoroutine(SpawnObjects());
    }
    Vector3 CoordGenerator()
    {
        float randX = Random.Range(-spawnRangeX, spawnRangeX);
        float randZ = Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnCoords = new Vector3(randX, 0, randZ);
        return spawnCoords;
    }
    IEnumerator SpawnObjects()
    {
        float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        if(juiceObjectCount < maxJuiceObjects)
        {
            Vector3 spawnCoords;
            int attempts = 0;
            do//runs once to check
            {
                spawnCoords = CoordGenerator();
                objectNearby = Physics.CheckSphere(spawnCoords, spawnRadius, spawnables);//bool to check for open spaces
                attempts++;
            } while (objectNearby && attempts < 5);// :/
            int randID = Random.Range(0, juiceObjects.Length);
            PhotonNetwork.Instantiate(Path.Combine("Spawn Objects", juiceObjects[randID].name), spawnCoords, Quaternion.identity);
            juiceObjectCount++;
        }
        if(obstacleCount < maxObstacles)
        {
            Vector3 spawnCoords = CoordGenerator();
            int randID = Random.Range(0, asteroids.Length);
            PhotonNetwork.Instantiate(Path.Combine("Spawn Objects", asteroids[randID].name), spawnCoords + (Vector3.up * asteroidYSpawnCoord), Quaternion.identity);
            obstacleCount++;
        }
        yield return new WaitForSeconds(spawnTime);
        
        StartCoroutine(SpawnObjects());
    }
}
