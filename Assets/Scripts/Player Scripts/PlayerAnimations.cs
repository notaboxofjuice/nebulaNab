using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimations : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Animator anime;

    [SerializeField]
    private Movement movementScript;

    [Space(5)]
    [Header("Particles")]
    [SerializeField]
    private GameObject flameEmmiter;
    [SerializeField]
    private GameObject flameEmmiterTwo;
   
    [SerializeField]
    private GameObject sparks;

    [Space(5)]
    [Header("Audios")]
    [SerializeField]
    private AudioClip exitShip;
    [SerializeField]
    private AudioClip hitByAsteroid;

    AudioSource audioPlayer;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();

        anime.SetBool("inSpace", false);
        flameEmmiter.SetActive(false);
        flameEmmiterTwo.SetActive(false);
        


        audioPlayer = GetComponent<AudioSource>();
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
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            anime.SetBool("inSpace", false);

            flameEmmiter.SetActive(false);
            flameEmmiterTwo.SetActive(false);

            
        }
    }

    public void CallStunnedRPC()
    {
        view.RPC("Stunned", RpcTarget.All);
    }

    public void CallRecoveredRPC()
    {
        view.RPC("Recover", RpcTarget.All);
    }



    [PunRPC]
    private void Stunned()
    {
        flameEmmiter.SetActive(false);
        flameEmmiterTwo.SetActive(false);

        sparks.SetActive(true);

        audioPlayer.PlayOneShot(hitByAsteroid);

        anime.SetBool("isPanicing", true);
    }

    [PunRPC]
    private void Recover()
    {
        flameEmmiter.SetActive(true);
        flameEmmiterTwo.SetActive(true);

        sparks.SetActive(false);

        anime.SetBool("isPanicing", false);
    }

}
