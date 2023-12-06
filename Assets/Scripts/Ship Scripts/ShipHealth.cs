
using UnityEngine;
using Photon.Pun;

public class ShipHealth : MonoBehaviourPunCallbacks
{
    private int currentHealth = 5;
    [SerializeField] private int maxHealth = 5;
    public string team;
    private void Start()
    {
        currentHealth = maxHealth;
    }
    [PunRPC]
    public void TakeDamage()
    {
        currentHealth--;
        GameUI.Instance.UpdateHealthUI(team, currentHealth);
        if(currentHealth <= 0)
        {
            PhotonView view = GameManager.Instance.GetComponent<PhotonView>();
            view.RPC("LoseGame", RpcTarget.All, team);
        }
    }
    public int GetHealth() 
    {
        return currentHealth;
    }
}
