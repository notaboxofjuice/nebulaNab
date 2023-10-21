using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun.Demo.Cockpit;

public class LobbyManager : MonoBehaviourPunCallbacks
{
   

    [SerializeField]
    private GameObject roomDisplayTab;

    [SerializeField]
    private GameObject optionsCanvas;
    [SerializeField]
    private GameObject searchRoomsCanvas;

    int roomCount = 0;

    //RoomId Input
    string inputedRoomID;
    [SerializeField]
    private TMP_InputField iDInputField;


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

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void SearchRooms()
    {
        optionsCanvas.SetActive(false);
        searchRoomsCanvas.SetActive(true);
    }

    public void BackToOptions()
    {
        optionsCanvas.SetActive(true);
        searchRoomsCanvas.SetActive(false);
    }


    public void JoinARoom()
    {
        if(inputedRoomID != "" || inputedRoomID != null)
        {
            PhotonNetwork.JoinRoom("Room:" + inputedRoomID);
        }
        
    }

    public void SetRoomIDNumber()
    {
        inputedRoomID = iDInputField.text;
    }


    #region Overrride Functions

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        PhotonNetwork.LoadLevel(2);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        SceneManager.LoadScene(2);
    }

    #endregion
}
