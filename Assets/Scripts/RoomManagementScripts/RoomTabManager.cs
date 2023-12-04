using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomTabManager : MonoBehaviourPunCallbacks
{//For the Room Prefab, gets and displays information for available rooms, allows players to join a room through button
    public Button joinButton;

    public TMP_Text playerCountText;

    public string _roomId;

    public int _playersInRoom = 0;

    public RoomInfo _roomInfo;

    public void SetRoomInfo(RoomInfo info)//Called by lobby manager
    {
        _roomInfo = info;
        playerCountText.text = "Players: " + info.PlayerCount + "/4";
        _roomId = info.Name.ToString();
    }

    public void JoinRoom()//called by button
    {
        if (_roomId != null || _roomId != "")
            PhotonNetwork.JoinRoom(_roomId);
        else
            Destroy(gameObject);//if Room Id is missing destroy this object so that player cannot try joining again
    }

    public override void OnJoinedRoom()
    {//load room scene once player has succeeded in joining a room
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(2);
    }
}
