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
            other.gameObject.GetComponent<JuiceInventory>().juiceCount += juiceCount;
            Destroy(gameObject);
        }
    }
}