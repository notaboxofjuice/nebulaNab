using Photon.Pun;
using UnityEngine;
/// <summary>
/// Assigned to player object
/// </summary>
public class OxygenTank : MonoBehaviour
{
    [SerializeField] int oxygen = 10; // how long player can survive on broken tank
    [SerializeField] CloneMachine cloneMachine; 
    [SerializeField] bool isBroken;
    
    [SerializeField] PlayerSpecialFX playerFX;
    public bool TankBroken { get { return isBroken; } }
    private void Start()
    {
        playerFX = GetComponent<PlayerSpecialFX>();
        FindFriendlyCloningMachine();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Door")) return; // if not colliding with door, do nothing
        else Restore(); // restore oxygen
    }
    [PunRPC]
    public void BreakTank() // called when another player actions this player
    {
        if (isBroken) return; // if already broken, do nothing
        Debug.Log("Breaking oxygen tank");
        isBroken = true; // set broken to true
        // Start timer for asphyxiation
        Invoke(nameof(Asphyxiate), oxygen);
        // FX
        playerFX.PlayTankBrokenSFX();//play sound effect
    }
    private void Asphyxiate()
    {
        // FX
        playerFX.PlayDeathSFX();//play sound effect before anything else
        playerFX.CheckJuiceAmount();

        Debug.Log("Player asphyxiated");
        // Try to send player to cloning machine
        cloneMachine.GetComponent<PhotonView>().RPC("TryAcceptCorpse", RpcTarget.All, GetComponent<PhotonView>().ViewID);
        // cloneMachine will handle the rest
       
        
    }
    public void Restore()
    {
        if (!isBroken) return; // if not cut, do nothing
        Debug.Log("Restoring oxygen");
        isBroken = false; // set cut to false
        CancelInvoke(nameof(Asphyxiate)); // Cancel asphyxiation timer
    }
    private void FindFriendlyCloningMachine()
    {
        // Find all clone machines
        CloneMachine[] cloneMachines = FindObjectsOfType<CloneMachine>();
        // Find the one on the same team
        foreach (CloneMachine machine in cloneMachines)
        {
            if (machine.Team == PhotonNetwork.LocalPlayer.GetTeam().ToString())
            {
                cloneMachine = machine;
                break;
            }
        }
    }
    [PunRPC]
    public void Respawn()
    {
        gameObject.SetActive(true);
    }
}