using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    

    [SerializeField]
    private RectTransform scrollViewContent;

    [SerializeField]
    private GameObject roomDisplayTab;

    int roomCount = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CreateRoom()
    {
        string roomName = "Room:" + Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }


    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        PhotonNetwork.LoadLevel(2);
    }
}
