using UnityEngine;
public class JuiceInventory : MonoBehaviour
{
    public int juiceCount;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Juice"))
        {
            juiceCount++;
            Destroy(collision.gameObject);
        }
    }
}