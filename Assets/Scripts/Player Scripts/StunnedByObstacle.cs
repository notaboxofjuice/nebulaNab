using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// To be assigned to Target object
/// Stuns Target and disables playerInput
/// when hit by an obstacle
/// </summary>
public class StunnedByObstacle : MonoBehaviour
{
    PlayerInput playerInput; // Movement script
    [SerializeField] float stunTime; // Time the Target is stunned for

    [SerializeField] PlayerSpecialFX playerAnimations;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); // Get the PlayerInput
        if (playerInput == null) Debug.LogError("PlayerInput not found");
    }

    private void OnTriggerEnter(Collider other)//trigger seemed to work without moving the Target
    {
        Debug.Log("trigger detected");
        if (other.gameObject.CompareTag("Obstacle")) // If the Target collides with an obstacle
        {
            Destroy(other.gameObject); // destroy the obstacle
            Debug.Log("Player is stunned");
            playerInput.enabled = false; // Disable playerInput

            if (playerAnimations.photonView.IsMine)
            {
                playerAnimations.CallStunnedRPC();
                
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