using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] float playerSpeed;
    [Header("Look Where Going")]
    [SerializeField] float rotationSpeed;
    private CharacterController cc;
    private Vector2 move;

    public float isMoving = 0;

    #endregion
    #region Methods
    #region Unity Methods
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        DoMovement();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door")) GetComponent<OxygenTank>().Restore(); // entering door, restore tether
    }
    #endregion
    #region My Methods
    public void TryMovement(InputAction.CallbackContext context)
    {
        //Debug.Log(PhotonNetwork.NickName + " is trying movement.");
        move = context.ReadValue<Vector2>();
        isMoving = move.magnitude; // YEFERSON'S CODE DO NOT DELETE FOR THE LOVE OF gud
    }
    private void DoMovement()
    {
        // calculate velocity relative to camera, ignoring camera's pitch
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = new(forward.z, 0, -forward.x);
        // calculate target velocity based on input
        Vector3 targetVelocity = (move.x * right + move.y * forward);
        targetVelocity.Normalize();
        targetVelocity.y = 0;
        // move the cc
        //cc.Move(playerSpeed * Time.deltaTime * targetVelocity);
        cc.SimpleMove(playerSpeed * targetVelocity); // use SimpleMove instead of Move so they go down
        if (isMoving > 0)
        {
            // rotate the cc in the direction of the velocity
            Quaternion targetRotation = Quaternion.LookRotation(targetVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    #endregion
    #endregion
}