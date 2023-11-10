using UnityEngine;
public class testCamera : MonoBehaviour
{
    [SerializeField]
    public GameObject player;//used by GameManager to assgin player upon instantiation
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    void LateUpdate()
    {
        transform.LookAt(player.transform.position);
    }
}