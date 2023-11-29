using Photon.Pun;
using UnityEngine;
/// <summary>
/// Can be assigned to any object that needs a juice inventory
/// </summary>
public class JuiceInventory : MonoBehaviour
{
    public int juiceCount;
    [Header("Tank Vars")]
    [SerializeField] private bool isTank; // is this a tank?
    public string TankTeam = null; // team of this tank
    private void OnTriggerEnter(Collider other) // for when assigned to juice obj
    {
        if (other.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Juice"))
        {
            Debug.Log(other.gameObject.name + " is collecting " + juiceCount + " juice."); // debug log
            other.gameObject.GetComponent<JuiceInventory>().juiceCount += juiceCount; // add juice to player
            ObjectSpawner.Instance.juiceObjectCount--; // decrement the juice object count
            Destroy(gameObject); // destroy the juice object
        }
    }
    [PunRPC]
    public void AcceptJuice(int acceptThis) // for accepting juice over network
    {
        juiceCount += acceptThis; // add juice to player
    }
}