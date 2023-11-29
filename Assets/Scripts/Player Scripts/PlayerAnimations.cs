using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
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
    [Header("Audio")]
    [SerializeField]
    private AudioClip exitShip;

    AudioSource audioPlayer;

    private void Start()
    {
        anime.SetBool("inSpace", false);
        flameEmmiter.SetActive(false);
        flameEmmiterTwo.SetActive(false);
        sparks.SetActive(false);

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
        audioPlayer.PlayOneShot(exitShip);
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


    public void Stunned()
    {
        flameEmmiter.SetActive(false);
        flameEmmiterTwo.SetActive(false);

        sparks.SetActive(true);

        anime.SetTrigger("Panic");
    }

    public void Recover()
    {
        flameEmmiter.SetActive(true);
        flameEmmiterTwo.SetActive(true);

        sparks.SetActive(false);

        anime.SetTrigger("Recovered");
    }

}
