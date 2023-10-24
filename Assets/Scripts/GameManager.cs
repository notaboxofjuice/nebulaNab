using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Transform blueSpawnPoint;
    [SerializeField]
    Transform redSpawnPoint;



    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        if(PhotonNetwork.LocalPlayer.GetTeam() == "Blue")
        {
            PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "Player"), blueSpawnPoint.position, blueSpawnPoint.rotation);
        }else if (PhotonNetwork.LocalPlayer.GetTeam() == "Red")
        {
            PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "Player"), redSpawnPoint.position, redSpawnPoint.rotation);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
