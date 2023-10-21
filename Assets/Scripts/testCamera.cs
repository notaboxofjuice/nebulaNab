using UnityEngine;
public class testCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    void LateUpdate()
    {
        transform.LookAt(player.transform.position);
    }
}