
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviourPunCallbacks
{
    public void SceneLoad(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
