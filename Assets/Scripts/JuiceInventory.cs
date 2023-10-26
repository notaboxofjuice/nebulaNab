using UnityEngine;
/// <summary>
/// Can be assigned to any object that needs a juice inventory
/// </summary>
public class JuiceInventory : MonoBehaviour
{
    public int juiceCount;
    private void OnCollisionEnter(Collision collision) // for when assigned to player
    {
        if (collision.gameObject.CompareTag("Juice"))
        {
            juiceCount++;
            Destroy(collision.gameObject);
        }
    }
}