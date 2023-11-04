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

    private bool joinedBlueTeam = false;
    private bool joinedRedTeam = false;

    GameObject localPlayer;

    [SerializeField]
    private int MaxPlayers = 4;
    public int currentRedPlayers = 0;
    public int currentBluePlayers = 0;



    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        roomIDText.text = PhotonNetwork.CurrentRoom.Name;//displays room ID on title text

        view = GetComponent<PhotonView>();

        PlayerTracker();

    }


    public void JoinBlueTeam()
    {
        if(!joinedBlueTeam && !joinedRedTeam)
        {
            //view.RPC("UpdateBlueTeam", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);

            var player = PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "GODCUBE"), blueSpawnPoint.position, blueSpawnPoint.rotation);
            localPlayer = player;

            joinedBlueTeam = true;

           

            PhotonNetwork.LocalPlayer.SetTeam("Blue");
            if(currentBluePlayers < 1)
            {
                PhotonNetwork.LocalPlayer.SetPlayerIndex(1);
            }
            else
            {
                PhotonNetwork.LocalPlayer.SetPlayerIndex(2);
            }


            view.RPC("AddBlueTeam", RpcTarget.AllBuffered);
        }
        else if (joinedRedTeam)
        {
            //view.RPC("RemoveName", RpcTarget.AllBuffered, redTeamPlayersText.ToString(),PhotonNetwork.LocalPlayer.NickName);
            if (localPlayer != null)
                localPlayer.transform.position = blueSpawnPoint.position;

            joinedBlueTeam = true;
            joinedRedTeam = false;

           

            PhotonNetwork.LocalPlayer.SetTeam("Blue");
            if (currentBluePlayers < 1)
            {
                PhotonNetwork.LocalPlayer.SetPlayerIndex(1);
            }
            else
            {
                PhotonNetwork.LocalPlayer.SetPlayerIndex(2);
            }

            view.RPC("AddBlueTeam", RpcTarget.AllBuffered);
            view.RPC("SubRedTeam", RpcTarget.AllBuffered);
        }
        PlayerTracker();

        Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerIndex() + " Index");
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


            PhotonNetwork.LocalPlayer.SetTeam("Red");
            if (currentRedPlayers < 1)
            {
                PhotonNetwork.LocalPlayer.SetPlayerIndex(1);

               
            }
            else
            {
                PhotonNetwork.LocalPlayer.SetPlayerIndex(2);

               
            }

            view.RPC("AddRedTeam", RpcTarget.AllBuffered);
        }
        else if (joinedBlueTeam)
        {
            
            if(localPlayer != null)
                localPlayer.transform.position = redSpawnPoint.position;

            joinedRedTeam = true;
            joinedBlueTeam = false;


            PhotonNetwork.LocalPlayer.SetTeam("Red");
            if (currentRedPlayers < 1)
            {
                PhotonNetwork.LocalPlayer.SetPlayerIndex(1);
            }
            else
            {
                PhotonNetwork.LocalPlayer.SetPlayerIndex(2);
            }

            view.RPC("AddRedTeam", RpcTarget.AllBuffered);
            view.RPC("SubBlueTeam", RpcTarget.AllBuffered);
        }
        PlayerTracker();

        Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerIndex() + " Index");
        view.RPC("CheckIfCanStartRPC", RpcTarget.AllBuffered);
    }

    private void PlayerTracker()
    {//will update display info whenever a player joins or leaves
        
        playerCount = PhotonNetwork.PlayerList.Length;

        currentPlayersText.text = "";

        for (int x = 0; x < playerCount; x++)
        {

          /* if(PhotonNetwork.PlayerList[x].GetTeam() == "Blue")
            {
                currentPlayersText.text += "<color=blue>"+ PhotonNetwork.PlayerList[x].NickName + "</color>" + "\n";
            }else if (PhotonNetwork.PlayerList[x].GetTeam() == "Red")
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
            if (currentBluePlayers == MaxPlayers / 2 && currentRedPlayers == MaxPlayers / 2)
            {
               
               startButton.SetActive(true);
                
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

    [PunRPC]
    void AddBlueTeam()
    {
        currentBluePlayers++;
    }
    [PunRPC]
    void SubBlueTeam()
    {
        currentBluePlayers--;
    }

    [PunRPC]
    void AddRedTeam()
    {
        currentRedPlayers++;
    }
    [PunRPC]
    void SubRedTeam()
    {
        currentRedPlayers--;
    }
}
