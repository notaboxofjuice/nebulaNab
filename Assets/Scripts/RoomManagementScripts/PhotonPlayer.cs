using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhotonPlayer : MonoBehaviour
{

    public static PhotonPlayer Instance;


    public int teamID;

    public string team;

    PhotonView view;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        view = GetComponent<PhotonView>();

        if (view != null && view.IsMine)
        {
            view.RPC("AssignTeamRPC", RpcTarget.AllBuffered);

            GetComponent<Movement>().enabled = true;
            GetComponent<PlayerInput>().enabled = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void AssignTeamRPC()
    {
        team = PhotonNetwork.LocalPlayer.GetTeam();
    }
}
