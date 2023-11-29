using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveLoop : MonoBehaviour
{
    public float speed = 10;
    [SerializeField]
    Animator anime;

    bool panicing = false;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-transform.right * Time.deltaTime * speed);

        if(transform.position.x > 50)
        {
            transform.position = new Vector3(-50, 2.7f, 11.3f);
            if (panicing)
            {
                anime.ResetTrigger("Panic");
                anime.SetTrigger("Recovered");
                panicing = false;
            }
            else
            {
                anime.ResetTrigger("Recovered");
                anime.SetTrigger("Panic");
                panicing=true;
            }
        }
    }
}
