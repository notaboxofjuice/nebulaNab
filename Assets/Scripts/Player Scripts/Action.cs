using UnityEngine;
using UnityEngine.InputSystem;
public class Action : MonoBehaviour
{
    #region Variables
    [Header("Gameplay Vars")]
    [SerializeField] GameObject daddy;
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
                hit.collider.gameObject.GetComponent<JuiceInventory>().juiceCount += daddy.GetComponent<JuiceInventory>().juiceCount;
                daddy.GetComponent<JuiceInventory>().juiceCount = 0;
            }
            else if (hit.collider.CompareTag("CloningMachine"))
            { // try cloning
                hit.collider.gameObject.GetComponent<CloneMachine>().TryClone();
            }
            else if (hit.collider.CompareTag("Cannon"))
            { // get cannon component and switch to cannon map
                this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Cannon");
                activeCannon = hit.collider.gameObject.GetComponent<Cannon>();
            }
        }
    }
    #endregion
    #region Cannon Actions
    public void AimCannon(InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>(); // read the button press
        activeCannon.Move(moveInput); // send the input to the cannon
    }
    public void FireCannon()
    {
        activeCannon.Fire();
    }
    public void ExitCannon()
    {
        activeCannon = null; // clear reference
        this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay"); // switch back to gameplay
    }
    #endregion
}