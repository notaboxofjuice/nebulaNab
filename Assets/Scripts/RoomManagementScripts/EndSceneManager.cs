using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {//release the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(this);//player leaves current room 
    }

    public override void OnLeftRoom()//once player has left room, return to lobby
    {
        base.OnLeftRoom();

        SceneManager.LoadScene(1);
    }
}
