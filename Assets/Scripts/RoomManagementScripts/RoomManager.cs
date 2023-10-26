using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("UI Refrences")]
    [SerializeField]
    private TMP_Text roomIDText;
    [SerializeField]
    private TMP_Text currentPlayersText;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private TMP_Text blueTeamPlayersText;
    [SerializeField]
    private TMP_Text redTeamPlayersText;

    private int playerCount = 0;
    bool startingGame = false;

    [SerializeField]
    private Transform blueSpawnPoint;
    [SerializeField]
    private Transform redSpawnPoint;

    PhotonView view;

    bool joinedBlueTeam = false;
    bool joinedRedTeam = false;

    GameObject localPlayer;

    PhotonTeamsManager teams;

    public int MaxPlayers = 4;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        roomIDText.text = PhotonNetwork.CurrentRoom.Name;//displays room ID on title text

        view = GetComponent<PhotonView>();

        PlayerTracker();

        teams = GetComponent<PhotonTeamsManager>();
    }


    public void JoinBlueTeam()
    {
        if(!joinedBlueTeam && !joinedRedTeam)
        {
            //view.RPC("UpdateBlueTeam", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);

            var player = PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "GODCUBE"), blueSpawnPoint.position, blueSpawnPoint.rotation);
            localPlayer = player;

            joinedBlueTeam = true;

            PhotonNetwork.LocalPlayer.JoinTeam(1);

            PhotonNetwork.LocalPlayer.SetTeam("Blue");
                 
        }
        else if (joinedRedTeam)
        {
            //view.RPC("RemoveName", RpcTarget.AllBuffered, redTeamPlayersText.ToString(),PhotonNetwork.LocalPlayer.NickName);
            if (localPlayer != null)
                localPlayer.transform.position = blueSpawnPoint.position;

            joinedBlueTeam = true;
            joinedRedTeam = false;

            PhotonNetwork.LocalPlayer.SwitchTeam(1);

            PhotonNetwork.LocalPlayer.SetTeam("Blue");
        }
        PlayerTracker();

        view.RPC("CheckIfCanStartRPC", RpcTarget.AllBuffered);
    }

    public void JoinRedTeam()
    {
        if(!joinedRedTeam && !joinedBlueTeam)
        {
            //view.RPC("UpdateRedTeam", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);

           var player = PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "GODCUBE"), redSpawnPoint.position, redSpawnPoint.rotation);

            localPlayer = player;
            joinedRedTeam = true;

            PhotonNetwork.LocalPlayer.JoinTeam(2);

            PhotonNetwork.LocalPlayer.SetTeam("Red");
        }
        else if (joinedBlueTeam)
        {
            
            if(localPlayer != null)
                localPlayer.transform.position = redSpawnPoint.position;

            joinedRedTeam = true;
            joinedBlueTeam = false;

            PhotonNetwork.LocalPlayer.SwitchTeam(2);

            PhotonNetwork.LocalPlayer.SetTeam("Red");
        }
        PlayerTracker();

        view.RPC("CheckIfCanStartRPC", RpcTarget.AllBuffered);
    }

    private void PlayerTracker()
    {//will update display info whenever a player joins or leaves
        
        playerCount = PhotonNetwork.PlayerList.Length;

        blueTeamPlayersText.text = "";
        redTeamPlayersText.text = "";

        currentPlayersText.text = "";

        for (int x = 0; x < playerCount; x++)
        {

           /* if ( PhotonNetwork.PlayerList[x].GetPhotonTeam())
            {
                currentPlayersText.text += "<color=blue>"+ PhotonNetwork.PlayerList[x].NickName + "</color>" + "\n";
            }else if (joinedRedTeam)
            {
                currentPlayersText.text += "<color=red>" + PhotonNetwork.PlayerList[x].NickName + "</color>" + "\n";
            }
            else
            {
                currentPlayersText.text += "<color=white>" + PhotonNetwork.PlayerList[x].NickName + "</color>" + "\n";
            }*/

            currentPlayersText.text += "<color=white>" + PhotonNetwork.PlayerList[x].NickName + "</color>" + "\n";
        }

       
    }

    void CheckIfCanStartGame()
    {
        if (playerCount == MaxPlayers)//when max players reached close room to prevent more players form joining
        {
            if (teams.TryGetTeamMembers(1, out Player[] blueMembers) && teams.TryGetTeamMembers(2, out Player[] redMembers))
            {
                if (blueMembers.Length == MaxPlayers/2 && redMembers.Length == MaxPlayers/2 && PhotonNetwork.IsMasterClient)
                {
                    startButton.SetActive(true);
                }
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
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
        view.RPC("CheckIfCanStartRPC", RpcTarget.AllBuffered);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Destroy(localPlayer);

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

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
      
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(0);
    }

    [PunRPC]
    void CheckIfCanStartRPC()
    {
        CheckIfCanStartGame();
    }
}
