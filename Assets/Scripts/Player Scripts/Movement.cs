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
        // rotate in the direction of move, relative to Camera.main.transform
        Vector3 _move = new Vector3(move.x, 0, move.y);
        Vector3 _direction = Camera.main.transform.TransformDirection(_move);
        _direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), rotationSpeed * Time.deltaTime);
        // move in the direction of move
        cc.SimpleMove(playerSpeed * _direction);
    }
    #endregion
    #endregion
}