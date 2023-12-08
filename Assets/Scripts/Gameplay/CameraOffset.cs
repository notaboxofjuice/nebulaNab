using UnityEngine;
public class CameraOffset : MonoBehaviour
{
    public GameObject Target; // Player object
    Vector3 startingPos; // Starting position of the camera
    public float minFollowDist; // Minimum distance the camera can be from the Target
    [SerializeField] float maxHeight; // Maximum height of the camera
    public float minHeight; // Height of the camera
    [SerializeField] float smoothTime; // Time it takes for the camera to move to the Target
    Vector3 velocity; // Velocity of the camera

    [SerializeField] bool inverted = false;

    void Start()
    {
        if (inverted) minFollowDist = -minFollowDist;//red team cam needs to face the oppiste direction
        startingPos = transform.position; // Set the starting position
        velocity = Vector3.zero; // Set the velocity to zero
    }
    void FixedUpdate()
    {
        float distance = Vector3.Distance(startingPos, Target.transform.position); // Get the distance between the startingPos and the Target
        distance *= 0.5f; // Multiply the distance by 0.5
        Vector3 targetPos = Target.transform.position + (Vector3.up * distance) - (Vector3.forward * minFollowDist);
        targetPos.y = Mathf.Clamp(targetPos.y, minHeight, maxHeight);
        // move and rotate
        transform.SetPositionAndRotation(Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime), Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target.transform.position - transform.position), smoothTime));
    }
}