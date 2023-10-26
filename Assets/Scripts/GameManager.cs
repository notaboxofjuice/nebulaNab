using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
     List<Transform> blueSpawnPoint = new List<Transform>();

    [SerializeField]
    List<Transform> redSpawnPoint = new List<Transform>();
   
    [SerializeField]
    Camera blueCam;
    [SerializeField]
    Camera redCam;
   


    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayers();

    }

    private void SpawnPlayers()
    {
        if (PhotonNetwork.LocalPlayer.GetTeam() == "Blue")
        {
            Vector3 spawnPoint = blueSpawnPoint[0].position;
            spawnPoint.x += Random.Range(-5, 5);

            PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "Player"), spawnPoint, blueSpawnPoint[0].rotation);
            blueCam.gameObject.SetActive(true);
            

        }
        else if (PhotonNetwork.LocalPlayer.GetTeam() == "Red")
        {
            Vector3 spawnPoint = redSpawnPoint[0].position;
            spawnPoint.x += Random.Range(-5, 5);

            PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "Player"), spawnPoint, redSpawnPoint[0].rotation);
            redCam.gameObject.SetActive(true);

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


   
}
