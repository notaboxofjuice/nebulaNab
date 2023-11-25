using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpecialFX : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Animator anime;
    [SerializeField]
    private Movement movementScript;//refrence to know when player is moving
    
    private GameManager gameManager;


    [Space(5)]
    [Header("Particles")]
    [SerializeField]
    private GameObject flameEmmiter;
    [SerializeField]
    private GameObject flameEmmiterTwo;
    [SerializeField]
    private GameObject sparks;//will appear when stunned

    [Space(5)]
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip exitShip;
    [SerializeField]
    private AudioClip hitByAsteroid;
    [SerializeField]
    private AudioClip collectJuice;
    [SerializeField]
    private AudioClip tankBroken;
    [SerializeField]
    private AudioClip death;
    [SerializeField]
    private AudioClip useCannon;
    [SerializeField]
    private AudioClip leaveCannon;
    [SerializeField]
    private AudioClip fireCannon;
    [SerializeField]
    private AudioClip activateClone;
    [SerializeField]
    private AudioClip depositJuice;

    public AudioSource audioPlayer;

    private PhotonView view;
    private void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        anime.SetBool("inSpace", false);
        flameEmmiter.SetActive(false);
        flameEmmiterTwo.SetActive(false);

        

    }

    private void Update()
    {
        if (movementScript.isMoving != 0)
        {
            anime.SetBool("isRunning", true);
        }
        else
        {
            anime.SetBool("isRunning", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            
          anime.SetBool("inSpace", true);
          flameEmmiter.SetActive(true);
          flameEmmiterTwo.SetActive(true);


          audioPlayer.PlayOneShot(exitShip);
          //add space sound effects
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door")) audioPlayer.PlayOneShot(exitShip);
        if (other.CompareTag("Juice")) audioPlayer.PlayOneShot(collectJuice);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Door"))
        {//when the player is inside the ship
            anime.SetBool("inSpace", false);

            flameEmmiter.SetActive(false);
            flameEmmiterTwo.SetActive(false);

            
        }
    }

    public void CheckJuiceAmount()
    {
        if (gameManager.myTeamJuiceInventory.juiceCount > gameManager.myCannon.fireCost)
        {
            view.RPC("CannonUsable", RpcTarget.All);
        }
        else
        {
            view.RPC("CannonNotUsable", RpcTarget.All);
        }

        if(gameManager.myTeamJuiceInventory.juiceCount > gameManager.myCloneMachine.cloneCost && gameManager.myCloneMachine.currentPlayer != null)
        {
            view.RPC("CloneMachineIsUsable", RpcTarget.All);
        }
        else
        {
            view.RPC("CloneManchineNotUsable", RpcTarget.All);
        }


    }

    #region Sound effects called by Action
    public void PlayTankBrokenSFX()
    {
        audioPlayer.PlayOneShot(tankBroken);
    }

    public void PlayDeathSFX()
    {
        audioPlayer.PlayOneShot(death);
    }

    public void PlayOperateCannon()
    {
        audioPlayer.PlayOneShot(useCannon);
    }

    public void PlayLeaveCannon()
    {
        audioPlayer.PlayOneShot(leaveCannon);
    }

    public void PlayFireCannon()
    {
        audioPlayer.PlayOneShot(fireCannon);
    }

    public void PlayDepositJuice()
    {
        audioPlayer.PlayOneShot(fireCannon);
    }

   

    #endregion

    #region RPCs

    public void CallStunnedRPC()
    {//other methods can call this non RPC mehtod, to call a RPC method, this way only this one needs a photon view refrence
        view.RPC("Stunned", RpcTarget.All);//Had to Rpc it, now all players see sparks on stunned player
    }

    public void CallRecoveredRPC()
    {
        view.RPC("Recover", RpcTarget.All);//with RPC, sparks turn off for all players on now recovered player
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

    [PunRPC]
    private void CloneMachineIsUsable()
    {
        gameManager.myCloneShimmer.SetActive(true);
    }
    [PunRPC]
    private void CloneManchineNotUsable()
    {
        gameManager.myCloneShimmer.SetActive(false);
    }

    [PunRPC]
    private void CannonUsable()
    {
        gameManager.myCannonShimmer.SetActive(true);
    }

    [PunRPC]
    private void CannonNotUsable()
    {
        gameManager.myCannonShimmer.SetActive(false);
    }


    [PunRPC]
    private void Stunned()
    {//by RPCing it, all players will be able to see when a player is stunned
        //disable booster fire and enbale sparks and play panic animation
        flameEmmiter.SetActive(false);
        flameEmmiterTwo.SetActive(false);

        sparks.SetActive(true);

        audioPlayer.PlayOneShot(hitByAsteroid);

        anime.SetBool("isPanicing", true);
    }

    [PunRPC]
    private void Recover()
    {//for when the player stunned finishes
        flameEmmiter.SetActive(true);
        flameEmmiterTwo.SetActive(true);

        sparks.SetActive(false);

        anime.SetBool("isPanicing", false);
    }
    #endregion
}
