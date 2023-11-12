using UnityEngine;
/// <summary>
/// Assigned to tether object
/// </summary>
public class Tether : MonoBehaviour
{
    [SerializeField] int oxygen;
    [SerializeField] GameObject player;
    [SerializeField] GameObject cloneMachine;
    [SerializeField] bool isCut;
    private void Awake()
    {
        #region Find Player
        player = transform.parent.gameObject; // get player object
        if (player == null) Debug.LogError("Player object not found");
        #endregion
        #region Find friendly Cloning Machine
        cloneMachine = null; // TODO: find friendly cloning machine
        if (cloneMachine == null) Debug.LogError("Cloning Machine not found");
        #endregion
    }
    public void Cut()
    {
        if (isCut) return; // if already cut, do nothing
        isCut = true; // set cut to true
        // Deactivate mesh renderer
        GetComponent<MeshRenderer>().enabled = false;
        // Start timer for asphyxiation
        Invoke(nameof(Asphyxiate), oxygen);
    }
    private void Asphyxiate()
    {
        Debug.Log("Player asphyxiated");
        // if both players are dead, lose

        // else, move player to cloning machine
        transform.parent.position = cloneMachine.transform.position;
        // deactivate player object
        player.SetActive(false);
    }
    public void Restore()
    {
        if (!isCut) return; // if not cut, do nothing
        isCut = false; // set cut to false
        // Cancel asphyxiation timer
        CancelInvoke(nameof(Asphyxiate));
        // Reactivate mesh renderer
        GetComponent<MeshRenderer>().enabled = true;
    }
}