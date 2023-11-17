using UnityEngine;
public class CloneMachine : MonoBehaviour
{
    [SerializeField] GameObject ship; // assigned in inspector
    [SerializeField] int cloneCost; // assigned in inspector
    public GameObject currentPlayer; // which player is going to be cloned
    public void TryClone()
    {
        if (ship.GetComponent<JuiceInventory>().juiceCount < cloneCost) return; // if not enough juice, do nothing
        ship.GetComponent<JuiceInventory>().juiceCount -= cloneCost; // subtract juice from ship
        DoClone(); // call DoClone
    }
    private void DoClone()
    {
        // logic for respawning player
    }
}