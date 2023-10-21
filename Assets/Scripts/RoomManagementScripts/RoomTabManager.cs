using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomTabManager : MonoBehaviourPunCallbacks
{
    public Button joinButton;
    public TMP_Text playerCountText;

    public string roomId;

    public int playersInRoom = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerCountText.text = "Players: " + playersInRoom;
    }

    public void JoinRoom()
    {
        if (roomId != null || roomId != "")
            PhotonNetwork.JoinRoom(roomId);
        else
            Destroy(gameObject);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(2);
    }
}
