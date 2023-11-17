using UnityEngine;
/// <summary>
/// Assigned to player object
/// </summary>
public class OxygenTank : MonoBehaviour
{
    [SerializeField] int oxygen;
    [SerializeField] GameObject cloneMachine;
    [SerializeField] bool isBroken;
    [SerializeField] GameObject door;
    private void Awake()
    {
        #region Find friendly Cloning Machine
        cloneMachine = null; // TODO: find friendly cloning machine
        if (cloneMachine == null) Debug.LogError("Cloning Machine not found");
        #endregion
    }
    private void OnTriggerEnter(Collider other)
    {
        if (door != null) return;
        else if (other.CompareTag("Door")) ;
    }
    public void BreakTank()
    {
        if (isBroken) return; // if already broken, do nothing
        isBroken = true; // set broken to true
        // Start timer for asphyxiation
        Invoke(nameof(Asphyxiate), oxygen);
    }
    private void Asphyxiate()
    {
        Debug.Log("Player asphyxiated");
        // if both players are dead, lose
        // TODO: check if both players are dead
        // else, move player to cloning machine
        transform.parent.position = cloneMachine.transform.position;
        cloneMachine.GetComponent<CloneMachine>().CurrentPlayer = gameObject;
        // deactivate player object
        gameObject.SetActive(false);
    }
    public void Restore()
    {
        if (!isBroken) return; // if not cut, do nothing
        isBroken = false; // set cut to false
        // Cancel asphyxiation timer
        CancelInvoke(nameof(Asphyxiate));
    }
}