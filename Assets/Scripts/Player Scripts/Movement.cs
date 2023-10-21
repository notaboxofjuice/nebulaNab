using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] float playerSpeed;
    [SerializeField] float maxVelocityChange;
    [Header("Look Where Going")]
    [SerializeField] GameObject playerBody;
    [SerializeField] float rotationSpeed;
    private Rigidbody rb;
    private Vector2 move;
    #endregion
    #region Methods
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        move = context.ReadValue<Vector2>();
    }
    private void DoMovement()
    {
        // calculate how fast we should be moving
        Vector3 targetVelocity = new(move.x, 0, move.y);
        // movement calculations
        targetVelocity = transform.TransformDirection(targetVelocity) * playerSpeed;
        // apply a force that attempts to reach our target velocity
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange); // add the velo
    }
    void LookWhereGoing()
    {
        Quaternion newRotation = Quaternion.LookRotation(rb.velocity);
        playerBody.transform.rotation = Quaternion.Slerp(playerBody.transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }
    #endregion
}