using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneMachinePlaySound : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private AudioClip activateClone;

    private PhotonView view;

    private AudioSource audioPlayer;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        view = GetComponent<PhotonView>();
    }
    public void PlayCloning()
    {
        view.RPC("Cloned", RpcTarget.All);
    }


    [PunRPC]
    private void Cloned()
    {
        audioPlayer.PlayOneShot(activateClone);

    }
}
