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

 
}
