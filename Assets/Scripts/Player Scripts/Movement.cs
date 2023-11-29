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
        // rotate in the direction of move
        if (move == Vector2.zero) return; // not moving, return
        float targetAngle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        // move in the direction of move
        cc.SimpleMove(move.magnitude * playerSpeed * transform.forward);
    }
    #endregion
    #endregion
}