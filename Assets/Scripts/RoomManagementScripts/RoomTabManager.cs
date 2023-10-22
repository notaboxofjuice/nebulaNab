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

    public string _roomId;

    public int _playersInRoom = 0;

    public RoomInfo _roomInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetRoomInfo(RoomInfo info)
    {
        _roomInfo = info;

        playerCountText.text = "Players: " + info.PlayerCount + "/4";

        _roomId = info.Name.ToString();
    }

    public void JoinRoom()
    {
        if (_roomId != null || _roomId != "")
            PhotonNetwork.JoinRoom(_roomId);
        else
            Destroy(gameObject);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(2);
    }
}
