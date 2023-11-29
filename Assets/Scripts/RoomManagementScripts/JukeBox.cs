using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class JukeBox : MonoBehaviour
{
    [SerializeField] AudioClip[] soundtrack;
    private AudioSource jukebox;
    private bool readyToPlay;
    private int index;
    [SerializeField]private bool inLobby = false;//check that we are not in lobby, where the volume is set :/
    // Start is called before the first frame update
    void Start()
    {

        jukebox = GetComponent<AudioSource>();

        if (!inLobby)//since volume is set in lobby, lobby manager handls volume control
        {
            float volume = PhotonNetwork.LocalPlayer.GetVolume();//get volume player set in lobby
            jukebox.volume = volume;//set the volume
            if (volume == 0) gameObject.SetActive(false);//if the volume is zero, diasble to the object, so that it isnt running needlessly

        }

        //index = Random.Range(0,soundtrack.Length -1);//will pick a random song to play at startup
        index = 0;
        readyToPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayNextSong();
    }

    void PlayNextSong()
    {
        if (readyToPlay)
        {
            if(index < soundtrack.Length - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            jukebox.clip = soundtrack[index];
            jukebox.Stop();//stops previous song, in case it wants to loop

            jukebox.Play();//start playing new song
            readyToPlay = false;
            StartCoroutine(WaitForFinish());
        }
    }
    
    IEnumerator WaitForFinish()
    {//play entire sone, then move onto the next one
        float seconds = soundtrack[index].length;
        yield return new WaitForSeconds(seconds);
        readyToPlay = true;
    }
}
