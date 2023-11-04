using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{//handles most basic game related stuff, like spawning in players, win/lose state. ect
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
        {//if the player is in blue team spawn them in blue area

            if (PhotonNetwork.LocalPlayer.GetPlayerIndex() == 1)//player index assgined in room is used to decide which character they get
            {
                PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "BluePlayerOne"), blueSpawnPoint[0].position, blueSpawnPoint[0].rotation);
              
            }
            else
            {
                PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "BluePlayerTwo"), blueSpawnPoint[1].position, blueSpawnPoint[1].rotation);
               
            }
            //enalble blue camera, disable red, incase it was enabled
            blueCam.gameObject.SetActive(true);
            redCam.gameObject.SetActive(false);

        }
        else if (PhotonNetwork.LocalPlayer.GetTeam() == "Red")
        {//if the player is in red team spawn them in red area
            
            if(PhotonNetwork.LocalPlayer.GetPlayerIndex() == 1)//player index assgined in room is used to decide which character they get
            {
                PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "RedPlayerOne"), redSpawnPoint[0].position, redSpawnPoint[0].rotation);
               
            }
            else
            {
                PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "RedPlayerTwo"), redSpawnPoint[1].position, redSpawnPoint[1].rotation);
               
            }
            //enalble red camera, disable blue, incase it was enabled
            redCam.gameObject.SetActive(true);
            blueCam.gameObject.SetActive(false);
        }
    }
   
}
