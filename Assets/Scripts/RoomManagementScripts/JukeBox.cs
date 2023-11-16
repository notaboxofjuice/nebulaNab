using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class JukeBox : MonoBehaviour
{
    [SerializeField] AudioClip[] soundtrack;
    AudioSource jukebox;
    bool readyToPlay;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        jukebox = GetComponent<AudioSource>();
        index = Random.Range(0,soundtrack.Length -1);//will pick a random song to play at startup
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
