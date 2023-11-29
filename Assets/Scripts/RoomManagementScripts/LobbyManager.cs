using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Canvas Refrences")]
    [SerializeField]
    private GameObject optionsCanvas;
    [SerializeField]
    private GameObject searchRoomsCanvas;
  
    [Space(5)]
    [Header("Display Rooms Refrences")]
    [SerializeField]
    private RoomTabManager roomDisplayTab;
    [SerializeField]
    private RectTransform tabContainer;
    private List<RoomTabManager> _currentListings = new List<RoomTabManager>();


  [Space(5)]
    [Header("Input Room Refrences")]
    //RoomId Input
    private string inputedRoomID;
    [SerializeField]
    private TMP_InputField iDInputField;

    [SerializeField]
    private TMP_InputField playerName_Input;
    string playerNickname;

    // Start is called before the first frame update
    void Start()
    {
        playerNickname = "Player" + Random.Range(0, 10000);

        PhotonNetwork.NickName = playerNickname;
    }

    public void UpdatePlayerNickname()
    {
        if(playerName_Input.text != "")
            playerNickname = playerName_Input.text;

        PhotonNetwork.NickName = playerNickname;
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {//updates the room list for the search rooms tab
        base.OnRoomListUpdate(roomList);
        
        foreach (RoomInfo roomInfo in roomList)
        {
            if(roomInfo.RemovedFromList)//if a room has been closed, delete from the display tab
            {
                int index = _currentListings.FindIndex(x => x._roomInfo.Name == roomInfo.Name);

                if(index != -1)
                {
                    Destroy(_currentListings[index].gameObject);
                    _currentListings.RemoveAt(index);
                }
            }
            else//if no room has been removed it means that some changed has occured, either a new room has been created, or a player has left or joined a room
            {
                int index = _currentListings.FindIndex(x => x._roomInfo.Name == roomInfo.Name);//returns -1 if no room of that name found


                if(index == -1)//if no tab for room exists create a new one//this means that a new room has been created
                {
                    RoomTabManager room = Instantiate(roomDisplayTab, tabContainer);

                    if (room != null)
                    {
                        room.SetRoomInfo(roomInfo);
                        _currentListings.Add(room);
                    }
                }
                else if(index != -1)//if room tab does exist update its information displayed//this mean a player has either joined or left a room
                {
                    _currentListings[index].SetRoomInfo(roomInfo);
                }

                
                   
            }

            
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        SceneManager.LoadScene(2);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        SceneManager.LoadScene(2);
    }

    #endregion
}
