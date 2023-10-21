using UnityEngine;
public class CloneMachine : MonoBehaviour
{
    [SerializeField] GameObject ship; // assigned in inspector
    [SerializeField] int cloneCost; // assigned in inspector
    public void TryClone()
    {
        if (ship.GetComponent<JuiceInventory>().juiceCount < cloneCost)
        { // not enough juice, do nothing
            return;
        } // enough juice, clone:
        ship.GetComponent<JuiceInventory>().juiceCount -= cloneCost;
        DoClone();
    }
    private void DoClone()
    {
        // logic for respawning player
    }
}