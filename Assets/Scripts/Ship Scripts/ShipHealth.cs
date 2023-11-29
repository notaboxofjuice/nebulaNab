
using UnityEngine;
using Photon.Pun;

public class ShipHealth : MonoBehaviourPunCallbacks
{
    private int currentHealth;
    [SerializeField] private int maxHealth = 5;
    [SerializeField] string team;
    private void Start()
    {
        currentHealth = maxHealth;
    }
    [PunRPC]
    public void TakeDamage()
    {
        currentHealth--;
        if(currentHealth <= 0)
        {
            GameManager.Instance.LoseGame(team);
        }
    }
    public int GetHealth() 
    {
        return currentHealth;
    }
}
