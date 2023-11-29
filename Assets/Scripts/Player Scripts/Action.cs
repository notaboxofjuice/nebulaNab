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
    [Header("FX")]
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
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayDistance))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);

            GameObject _hitObject = hit.collider.gameObject;
            PhotonView _hitPhotonView = _hitObject.GetComponent<PhotonView>();

            if (_hitObject.CompareTag("JuiceTank")) // deposit juice
            {
                Debug.Log(PhotonNetwork.NickName + " is depositing juice.");
                // Logic
                _hitPhotonView.RPC("AcceptJuice", RpcTarget.All, GetComponent<JuiceInventory>().juiceCount);
                GetComponent<JuiceInventory>().juiceCount = 0;
                // FX
                playerFX.PlayDepositJuice();
                playerFX.CheckJuiceAmount();
            }
            else if (_hitObject.CompareTag("CloningMachine")) // try cloning
            {
                Debug.Log(PhotonNetwork.NickName + " is trying to clone.");
                // Logic
                _hitPhotonView.RPC("TryClone", RpcTarget.All);
                // FX
                playerFX.PlayCloning();
                playerFX.CheckJuiceAmount();

            }
            else if (_hitObject.CompareTag("Cannon")) // get cannon component and switch to cannon map
            {
                Debug.Log(PhotonNetwork.NickName + " is entering cannon.");
                // Logic
                Cannon _hitCannon = _hitObject.GetComponent<Cannon>(); // cache cannon component
                if (_hitCannon.inUse) return; // if cannon is in use, do nothing
                _hitPhotonView.RPC("FlipOccupiedBool", RpcTarget.All);
                GetComponent<PlayerInput>().SwitchCurrentActionMap("Cannon"); // switch to cannon map
                activeCannon = _hitCannon; // set active cannon
                // FX
                playerFX.PlayOperateCannon();
            }
            else if (hit.collider.CompareTag("Player")) // break player's oxygen
            {
                Debug.Log(PhotonNetwork.NickName + " is breaking an oxygen tank.");
                // break tank via RPC
                _hitPhotonView.RPC("BreakTank", RpcTarget.All);
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
        activeCannon.GetComponent<PhotonView>().RPC("Fire", RpcTarget.All);
        // FX
        playerFX.PlayFireCannon();
        playerFX.CheckJuiceAmount();
    }
    public void ExitCannon()
    {
        // debug log the player's photon name
        Debug.Log(PhotonNetwork.NickName + " is exiting cannon.");
        activeCannon.GetComponent<PhotonView>().RPC("FlipOccupiedBool", RpcTarget.All);
        activeCannon = null; // clear reference
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay"); // switch back to gameplay
        // FX
        playerFX.PlayLeaveCannon();
    }
    #endregion
}