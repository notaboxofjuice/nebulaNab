using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// Yeferson: Managing teams and spawning players
/// Leeman: Managing game over states
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks
{
    #region Variables
    public static GameManager Instance { get; private set; } // allows referring to as GameManager.Instance
    [SerializeField] SceneLoader sceneLoader;
    #region Player Vars
    [Header("Spawn Points")]
    [SerializeField]
     List<Transform> blueSpawnPoint = new();

    [SerializeField]
    List<Transform> redSpawnPoint = new();
    [Header("Cameras")]
    [SerializeField]
    CameraOffset blueCam;
    [SerializeField]
    CameraOffset redCam;

    [Space(5)]
    [Header("Red Team Components")]
    [SerializeField]
    JuiceInventory redJuiceInventory;
    [SerializeField]
    private CloneMachine redCloneM;
    [SerializeField]
    private CannonControl redCannon;
    [SerializeField]
    GameObject redCannonShimmerEffect;//lets players know that cannon is usable
    [SerializeField]
    GameObject redCloneShimmerEffect;//lets players know that clone is usable//only if a Target is dead??

    [Space(5)]
    [Header("Blue Team Components")]
    [SerializeField]
    JuiceInventory blueJuiceInventory;
    [SerializeField]
    private CannonControl blueCannon;
    [SerializeField]
    private CloneMachine blueCloneM;
    [SerializeField]
    GameObject blueCannonShimmerEffect;//lets players know that cannon is usable
    [SerializeField]
    GameObject blueCloneShimmerEffect;//lets players know that clone is usable//only if a Target is dead??

    [Space(5)]
    [Header("Player Components- Assigned on Start Up")]
    [SerializeField] public CannonControl myCannon;//quick access for other scripts
    [SerializeField] public JuiceInventory myTeamJuiceInventory;
    [SerializeField] public CloneMachine myCloneMachine;//quick access for other scripts
    [SerializeField] public GameObject myCannonShimmer;
    [SerializeField] public GameObject myCloneShimmer;

    #endregion
    #region Win/Lose Vars

    #endregion
    #endregion
    #region Methods
    #region Unity Methods
    private void Awake()
    {
        // Singleton pattern
        if (Instance != null) Debug.LogError("More than one GameManager in scene");
        else Instance = this;
    }
    void Start()
    {
        SpawnPlayers();

        myTeamJuiceInventory = PhotonNetwork.LocalPlayer.GetTeam() == "Red" ? redJuiceInventory : blueJuiceInventory;
        myCloneMachine = PhotonNetwork.LocalPlayer.GetTeam() == "Red" ? redCloneM : blueCloneM;
        myCannon = PhotonNetwork.LocalPlayer.GetTeam() == "Red" ? redCannon : blueCannon;
        myCloneShimmer = PhotonNetwork.LocalPlayer.GetTeam() == "Red" ? redCloneShimmerEffect : blueCloneShimmerEffect;
        myCannonShimmer = PhotonNetwork.LocalPlayer.GetTeam() == "Red" ? redCannonShimmerEffect : blueCannonShimmerEffect;
        PhotonNetwork.AutomaticallySyncScene = false;
    }
    #endregion
    #region Spawning Methods
    private void SpawnPlayers()
    {
        if (PhotonNetwork.LocalPlayer.GetTeam() == "Blue")
        {//if the Target is in blue team spawn them in blue area

            if (PhotonNetwork.LocalPlayer.GetPlayerIndex() == 1)//Target index assgined in room is used to decide which character they get
            {
                var player = PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "BluePlayerOne"), blueSpawnPoint[0].position, blueSpawnPoint[0].rotation);
                blueCam.Target = player;

            }
            else
            {
                var player = PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "BluePlayerTwo"), blueSpawnPoint[1].position, blueSpawnPoint[1].rotation);
                blueCam.Target = player;
            }
            //enable blue camera, disable red, incase it was enabled
            blueCam.gameObject.SetActive(true);

            redCam.gameObject.SetActive(false);

        }
        else if (PhotonNetwork.LocalPlayer.GetTeam() == "Red")
        {//if the Target is in red team spawn them in red area
            
            if(PhotonNetwork.LocalPlayer.GetPlayerIndex() == 1)//Target index assgined in room is used to decide which character they get
            {
                var player = PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "RedPlayerOne"), redSpawnPoint[0].position, redSpawnPoint[0].rotation);
               redCam.Target = player;
            }
            else
            {
                var player = PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "RedPlayerTwo"), redSpawnPoint[1].position, redSpawnPoint[1].rotation);
                redCam.Target = player;
            }
            //enalble red camera, disable blue, incase it was enabled
            redCam.gameObject.SetActive(true);
            blueCam.gameObject.SetActive(false);
        }
    }
    #endregion
    #region Win/Lose Methods
    
    [PunRPC]
    public void LoseGame(string LosingTeam) // called when a team loses
    {
        Debug.Log("Losing team: " + LosingTeam);
        // Loop through players in lobby
      
        // If Target is on losing team
        if (PhotonNetwork.LocalPlayer.GetTeam() == LosingTeam)
        {
                sceneLoader.SceneLoad(5);
        }
        else
        {
                sceneLoader.SceneLoad(4);
        }
        
    }
    #endregion
    #endregion
}