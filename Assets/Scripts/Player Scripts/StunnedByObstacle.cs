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
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); // Get the playerInput script
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) // If the player collides with an obstacle
        {
            playerInput.enabled = false; // Disable playerInput
            Invoke(nameof(EnableMovement), stunTime); // Enable playerInput after stunTime seconds
        }
    }
    void EnableMovement()
    {
        playerInput.enabled = true; // Enable playerInput
    }
}