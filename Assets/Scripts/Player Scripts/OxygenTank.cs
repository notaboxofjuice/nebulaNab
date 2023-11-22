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
        #region Find friendly Cloning Machine
        // loop through and grab the one with desired team
        foreach (CloneMachine cm in FindObjectsOfType<CloneMachine>()) if (cm.Team == PhotonNetwork.LocalPlayer.GetTeam()) cloneMachine = cm;
        if (cloneMachine == null) Debug.LogError("Failed to find " + PhotonNetwork.LocalPlayer.GetTeam() + " cloning machine");
        #endregion
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Door")) return; // if not colliding with door, do nothing
        else Restore(); // restore oxygen
    }
    public void BreakTank() // called when another player actions this player
    {
        if (isBroken) return; // if already broken, do nothing
        Debug.Log("Breaking oxygen tank");
        isBroken = true; // set broken to true
        // Start timer for asphyxiation
        Invoke(nameof(Asphyxiate), oxygen);

        playerFX.PlayTankBrokenSFX();//play sound effect
    }
    private void Asphyxiate()
    {
        Debug.Log("Player asphyxiated");
        // Try to send player to cloning machine
        cloneMachine.TryAcceptCorpse(gameObject);
        // cloneMachine will handle the rest

        playerFX.PlayDeathSFX();//play sound effect
    }
    public void Restore()
    {
        if (!isBroken) return; // if not cut, do nothing
        Debug.Log("Restoring oxygen");
        isBroken = false; // set cut to false
        CancelInvoke(nameof(Asphyxiate)); // Cancel asphyxiation timer
    }
}