using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// To be assigned to player object
/// Stuns player and disables playerInput
/// when hit by an obstacle
/// </summary>
public class StunnedByObstacle : MonoBehaviour
{
    PlayerInput playerInput; // Movement script
    [SerializeField] float stunTime; // Time the player is stunned for

    [SerializeField] PlayerAnimations playerAnimations;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); // Get the PlayerInput
        if (playerInput == null) Debug.LogError("PlayerInput not found");
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.CompareTag("Obstacle")) // If the player collides with an obstacle
        {
            Debug.Log("Player is stunned");
            playerInput.enabled = false; // Disable playerInput
            Invoke(nameof(EnableMovement), stunTime); // Enable playerInput after stunTime seconds
        }
    }
    */

    private void OnTriggerEnter(Collider other)//trigger seemed to work without moving the player
    {
        Debug.Log("trigger detected");
        if (other.gameObject.CompareTag("Obstacle")) // If the player collides with an obstacle
        {
            Destroy(other.gameObject); // destroy the obstacle
            Debug.Log("Player is stunned");
            playerInput.enabled = false; // Disable playerInput

            if (playerAnimations.photonView.IsMine)
            {
                playerAnimations.CallStunnedRPC();
                Debug.Log("I Sghould only be one");
            }
                

            Invoke(nameof(EnableMovement), stunTime); // Enable playerInput after stunTime seconds
        }
    }

    void EnableMovement()
    {
        Debug.Log("Player is no longer stunned");
        playerInput.enabled = true; // Enable playerInput

        if (playerAnimations.photonView.IsMine)
        {
            playerAnimations.CallRecoveredRPC();
            Debug.Log("Recovered");
        }
            
    }
}