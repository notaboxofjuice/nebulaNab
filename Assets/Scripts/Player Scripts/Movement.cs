using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] float playerSpeed;
    [Header("Look Where Going")]
    [SerializeField] Transform bodyTransform;
    [SerializeField] float rotationSpeed;
    private CharacterController cc;
    private Vector2 move;
    #endregion
    #region Methods
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        DoMovement();
    }
    private void LateUpdate()
    {
        if (transform.hasChanged)
        {
            LookWhereGoing();
            transform.hasChanged = false;
        }
    }
    public void TryMovement(InputAction.CallbackContext context)
    {
        Debug.Log(PhotonNetwork.NickName + " is trying movement.");
        move = context.ReadValue<Vector2>();
    }
    private void DoMovement()
    {
        // calculate velocity relative to camera, ignoring camera's pitch
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        // calculate target velocity based on input
        Vector3 targetVelocity = (move.x * right + move.y * forward);
        targetVelocity.Normalize();
        targetVelocity.y = 0;
        // move the cc
        cc.Move(targetVelocity * Time.deltaTime * playerSpeed);
    }
    void LookWhereGoing()
    {
        Quaternion newRotation = Quaternion.LookRotation(cc.velocity);
        bodyTransform.rotation = Quaternion.Slerp(bodyTransform.transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }
    #endregion
}