using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
public class Action : MonoBehaviour
{
    #region Variables
    [Header("Gameplay Vars")]
    [SerializeField] float rayDistance;
    [Header("Cannon Vars")]
    [SerializeField] Cannon activeCannon;
    #endregion
    #region Gameplay Actions
    public void GameplayAction() // called by input system
    { // raycast forward
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.collider.CompareTag("JuiceTank"))
            { // deposit juice
                Debug.Log(PhotonNetwork.NickName + " is depositing juice.");
                hit.collider.gameObject.GetComponent<JuiceInventory>().juiceCount += GetComponent<JuiceInventory>().juiceCount;
                GetComponent<JuiceInventory>().juiceCount = 0;
            }
            else if (hit.collider.CompareTag("CloningMachine"))
            { // try cloning
                Debug.Log(PhotonNetwork.NickName + " is trying to clone.");
                hit.collider.gameObject.GetComponent<CloneMachine>().TryClone();
            }
            else if (hit.collider.CompareTag("Cannon"))
            { // get cannon component and switch to cannon map
                Debug.Log(PhotonNetwork.NickName + " is entering cannon.");
                GetComponent<PlayerInput>().SwitchCurrentActionMap("Cannon");
                activeCannon = hit.collider.gameObject.GetComponent<Cannon>();
            }
        }
    }
    #endregion
    #region Cannon Actions
    public void AimCannon(InputAction.CallbackContext context)
    {
        Debug.Log(PhotonNetwork.NickName + " is aiming cannon.");
        float moveInput = context.ReadValue<float>(); // read the button press
        activeCannon.Move(moveInput); // send the input to the cannon
    }
    public void FireCannon()
    {
        Debug.Log(PhotonNetwork.NickName + " is firing cannon.");
        activeCannon.Fire();
    }
    public void ExitCannon()
    {
        // debug log the player's photon name
        Debug.Log(PhotonNetwork.NickName + " is exiting cannon.");
        activeCannon = null; // clear reference
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay"); // switch back to gameplay
    }
    #endregion
}