using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
public class Action : MonoBehaviourPunCallbacks
{
    #region Variables
    [HideInInspector] public JuiceInventory shipJuice; // ship's juice inventory, assigned from JuiceInventory.cs
    [Header("Gameplay Vars")]
    [SerializeField] float rayDistance;
    [Header("Cannon Vars")]
    [SerializeField] Cannon activeCannon;

    [SerializeField] PlayerSpecialFX playerFX;
    #endregion
    #region Unity Methods
    private void Start()
    {
        playerFX = GetComponent<PlayerSpecialFX>();
    }


    #endregion
    #region Gameplay Actions
    public void GameplayAction() // called by input system
    { // raycast forward
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.collider.CompareTag("JuiceTank")) // deposit juice
            {
                Debug.Log(PhotonNetwork.NickName + " is depositing juice.");
                shipJuice.juiceCount += GetComponent<JuiceInventory>().juiceCount;
                GetComponent<JuiceInventory>().juiceCount = 0;

                playerFX.PlayDepositJuice();
            }
            else if (hit.collider.CompareTag("CloningMachine")) // try cloning
            {
                Debug.Log(PhotonNetwork.NickName + " is trying to clone.");
                hit.collider.gameObject.GetComponent<CloneMachine>().TryClone();

                playerFX.PlayCloning();
            }
            else if (hit.collider.CompareTag("Cannon")) // get cannon component and switch to cannon map
            {
                Debug.Log(PhotonNetwork.NickName + " is entering cannon.");
                if (hit.collider.gameObject.GetComponent<Cannon>().inUse) return; // if cannon is in use, do nothing
                GetComponent<PlayerInput>().SwitchCurrentActionMap("Cannon"); // switch to cannon map
                activeCannon = hit.collider.gameObject.GetComponent<Cannon>(); // set active cannon
                activeCannon.inUse = true; // set inUse to true

                playerFX.PlayOperateCannon();
            }
            else if (hit.collider.CompareTag("Player")) // break player's oxygen
            {
                Debug.Log(PhotonNetwork.NickName + " is breaking an oxygen tank.");
                // break tank via RPC
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("BreakOtherTank", RpcTarget.All);
            }
            else Debug.Log("Hit object with tag: " + hit.collider.tag);
        }
    }
    #endregion
    #region Cannon Actions
    public void AimCannon(InputAction.CallbackContext context)
    {
        Debug.Log(PhotonNetwork.NickName + " is aiming cannon.");
        activeCannon.moveInput = context.ReadValue<float>(); // read and send the input to the cannon
    }
    public void FireCannon()
    {
        Debug.Log(PhotonNetwork.NickName + " is firing cannon.");
        activeCannon.Fire();
        playerFX.PlayFireCannon();
    }
    public void ExitCannon()
    {
        // debug log the player's photon name
        Debug.Log(PhotonNetwork.NickName + " is exiting cannon.");
        activeCannon.inUse = false; // set inUse to false
        activeCannon = null; // clear reference
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay"); // switch back to gameplay

        playerFX.PlayLeaveCannon();
    }
    #endregion
    #region I hate RPCs
    [PunRPC]
    public void BreakOtherTank()
    {  
        GetComponent<OxygenTank>().BreakTank(); // please god work
    }
    #endregion
}