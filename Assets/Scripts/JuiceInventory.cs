using UnityEngine;
/// <summary>
/// Can be assigned to any object that needs a juice inventory
/// </summary>
public class JuiceInventory : MonoBehaviour
{
    public int juiceCount;
    private void OnTriggerEnter(Collider other) // for when assigned to juice obj
    {
        if (other.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Juice"))
        {
            Debug.Log(other.gameObject.name + " is collecting " + juiceCount + " juice."); // debug log
            other.gameObject.GetComponent<JuiceInventory>().juiceCount += juiceCount; // add juice to player
            ObjectSpawner.Instance.juiceObjectCount--; // decrement the juice object count
            Destroy(gameObject); // destroy the juice object
        }
        /*
        if(other.gameObject.CompareTag("Player") && this.gameObject.CompareTag("JuiceTank"))
        {
            Debug.Log(other.gameObject.name + " is depositing " + other.gameObject.GetComponent<JuiceInventory>().juiceCount + " juice into tank.");
            this.juiceCount += other.gameObject.GetComponent<JuiceInventory>().juiceCount; //add player juice to tank
            other.gameObject.GetComponent<JuiceInventory>().juiceCount = 0; //empty player's inventory
        }
        */
    }
}