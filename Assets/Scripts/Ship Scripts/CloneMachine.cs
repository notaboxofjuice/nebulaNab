using Photon.Pun;
using UnityEngine;
/// <summary>
/// Assigned to clone machine object. Accepts dead players and respawns them
/// </summary>
public class CloneMachine : MonoBehaviourPunCallbacks
{
    public string Team; // team of this clone machine
    [SerializeField] JuiceInventory shipJuice; // ship's juice inventory
    [SerializeField] public int cloneCost; // cost to clone player, assigned in inspector
    public GameObject currentPlayer = null; // which player is going to be cloned
    [SerializeField] Transform spawnPoint; // where to spawn player
    [PunRPC]
    public void TryClone() // Try to clone currentPlayer, called by player action
    {
        if (currentPlayer == null) return; // if no currentPlayer, do nothing
        Debug.Log("Trying to clone...");
        if (shipJuice.juiceCount < cloneCost) Debug.Log("Not enough juice"); // if not enough juice, do nothing
        else DoClone(); // call DoClone
    }
    [PunRPC]
    public void TryAcceptCorpse(GameObject Corpse) // Try to accept a new corpse
    {
        Debug.Log("Trying to accept corpse...");
        // If teammate is already dead, that team loses
        if (currentPlayer != null) GameManager.Instance.LoseGame(PhotonNetwork.LocalPlayer.GetTeam());
        // Otherwise, accept the corpse
        else AcceptCorpse(Corpse);
    }
    private void AcceptCorpse(GameObject Corpse) // Accept a new corpse
    {
        Debug.Log("Accepting corpse");
        currentPlayer = Corpse; // set currentPlayer to Corpse
        currentPlayer.transform.SetLocalPositionAndRotation(spawnPoint.position, spawnPoint.rotation); // move currentPlayer to spawnPoint
        currentPlayer.SetActive(false); // disable currentPlayer
    }
    private void DoClone() // logic for respawning currentPlayer, called by TryClone
    {
        Debug.Log("Cloning player");
        shipJuice.juiceCount -= cloneCost; // subtract juice from ship
        currentPlayer.SetActive(true); // enable currentPlayer
        currentPlayer = null; // clear reference to currentPlayer
    }
}