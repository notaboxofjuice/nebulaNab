
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void SceneLoad(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
