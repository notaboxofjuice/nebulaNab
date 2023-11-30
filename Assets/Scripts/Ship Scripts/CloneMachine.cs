using Photon.Pun;
using System.Collections;
using UnityEngine;
/// <summary>
/// Assigned to clone machine object. Accepts dead players and respawns them
/// </summary>
public class CloneMachine : MonoBehaviourPunCallbacks, IPunObservable
{
    public string Team; // team of this clone machine
    [SerializeField] JuiceInventory shipJuice; // ship's juice inventory
    [SerializeField] public int cloneCost; // cost to clone player, assigned in inspector
    public GameObject currentPlayer = null; // which player is going to be cloned
    [SerializeField] Transform spawnPoint; // where to spawn player

    [SerializeField] Animator anime;

    [PunRPC]
    public void TryClone() // Try to clone currentPlayer, called by player action
    {
        if (currentPlayer == null) return; // if no currentPlayer, do nothing
        Debug.Log("Trying to clone...");
        if (shipJuice.juiceCount < cloneCost) Debug.Log("Not enough juice"); // if not enough juice, do nothing
        else DoClone(); // call DoClone
    }
    [PunRPC]
    public void TryAcceptCorpse(int ViewID) // Try to accept a new corpse
    {
        Debug.Log("Trying to accept corpse...");
        GameObject _corpse = PhotonView.Find(ViewID).gameObject; // cache corpse
        // If teammate is already dead, that team loses
        if (currentPlayer != null) GameManager.Instance.LoseGame(PhotonNetwork.LocalPlayer.GetTeam());
        // Otherwise, accept the corpse
        else AcceptCorpse(_corpse);
    }
    private void AcceptCorpse(GameObject Corpse) // Accept a new corpse
    {
        Debug.Log("Accepting corpse");
        currentPlayer = Corpse; // set currentPlayer to Corpse
        currentPlayer.transform.SetLocalPositionAndRotation(spawnPoint.position, spawnPoint.rotation); // move currentPlayer to spawnPoint
        
       
        currentPlayer.GetComponent<CharacterController>().enabled = false;
        currentPlayer.GetComponent<Action>().enabled = false;
        currentPlayer.GetComponent<PlayerSpecialFX>().enabled = false;
        currentPlayer.transform.GetChild(0).gameObject.SetActive(false);
        //currentPlayer.SetActive(false); // disable currentPlayer
    }
    private void DoClone() // logic for respawning currentPlayer, called by TryClone
    {
        Debug.Log("Cloning player");
        shipJuice.GetComponent<PhotonView>().RPC("AcceptJuice", RpcTarget.All, -cloneCost); // subtract juice from ship
        
        //currentPlayer.SetActive(true); // enable currentPlayer

        
        currentPlayer.GetComponent<CharacterController>().enabled = true;
        currentPlayer.GetComponent<Action>().enabled = true;
        currentPlayer.GetComponent<PlayerSpecialFX>().enabled = true;
        currentPlayer.transform.GetChild(0).gameObject.SetActive(true);

        currentPlayer = null; // clear reference to currentPlayer

        anime.SetBool("Close", false);
        anime.SetTrigger("Open");//play opening animation the machine
        StartCoroutine(close());
    }
    // Implement the OnPhotonSerializeView() method
    // idk if this does anything but bing ai said to put it here
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // If the stream is writing, write the currentPlayer's PhotonView ID
        {
            int viewID = currentPlayer == null ? -1 : currentPlayer.GetComponent<PhotonView>().ViewID; // If currentPlayer is null, write -1, otherwise write the viewID
            stream.SendNext(viewID); // Write the viewID to the stream
        }
        else // If the stream is reading, read the currentPlayer's PhotonView ID
        {
            int viewID = (int)stream.ReceiveNext(); // Read the viewID from the stream
            if (viewID == -1) // If the viewID is -1, set currentPlayer to null
            {
                currentPlayer = null;
            }
            else // Otherwise, find the game object with the viewID and set it as currentPlayer
            {
                currentPlayer = PhotonView.Find(viewID).gameObject;
            }
        }
    }

    IEnumerator close()
    {
        yield return new WaitForSeconds(2);
        anime.SetBool("Close", true);
    }

}