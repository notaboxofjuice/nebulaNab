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

    bool inSpace = false;

    private void Start()
    {
        anime.SetBool("inSpace", false);
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
          inSpace = true;
            
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            anime.SetBool("inSpace", false);
            inSpace = false;

        }
    }

}
