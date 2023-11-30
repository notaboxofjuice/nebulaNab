using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhotonPlayer : MonoBehaviourPunCallbacks
{

    public static PhotonPlayer Instance;

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
            GetComponent<PlayerSpecialFX>().enabled = true;

            transform.GetChild(1).gameObject.SetActive(true);
        }
        
    }
    [PunRPC]
    void AssignTeamRPC()
    {
        team = PhotonNetwork.LocalPlayer.GetTeam();
    }
}
