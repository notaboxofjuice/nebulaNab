using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("UI Refrences")]
    [SerializeField]
    [Tooltip("Shows players the current room ID")]
    private TMP_Text roomIDText;
    [SerializeField]
    [Tooltip("inactive until the the room has 4 players")]
    GameObject startButton;

    private int playerCount = 0;

   bool startingGame = false;

    // Start is called before the first frame update
    void Start()
    {
        roomIDText.text = PhotonNetwork.CurrentRoom.Name;//displays room ID on title text
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void PlayerTracker()
    {//will update display info whenever a player joins or leaves
        playerCount = PhotonNetwork.PlayerList.Length;

        if(playerCount == PhotonNetwork.CurrentRoom.MaxPlayers)//when max players reached close room to prevent more players form joining
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            startButton.SetActive(true);
        }
        else
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            startButton.SetActive(false);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
        PlayerTracker();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        PlayerTracker();
    }

    public void StartGame()
    {
        startingGame = true;

        if (!PhotonNetwork.IsMasterClient)
            return;

        startButton.SetActive(false);
        PhotonNetwork.LoadLevel(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
