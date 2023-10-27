using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class Cannon : MonoBehaviour
{
    PhotonView view;
    PhotonTransformView transformView;
    [Tooltip("The Ship Object this cannon is attached to.")]
    [SerializeField] GameObject ship;
    [Tooltip("The laser prefab to be instantiated when the cannon fires.")]
    [SerializeField] GameObject laser;
    [Tooltip("How far in front of the cannon to spawn the laser.")]
    [SerializeField] float spawnOffset = 1f;
    [SerializeField] int fireCost = 15;
    [Tooltip("How far left or right the cannon can move as a float. Negative values will be converted to positive.")]
    [SerializeField] float movementRange = 10f;
    [Tooltip("How fast the cannon moves as a float")]
    [SerializeField] float movementSpeed = 2f;
    JuiceInventory juiceInventory;
    private void Awake()
    {
        view = gameObject.GetComponent<PhotonView>();
        transformView = gameObject.GetComponent<PhotonTransformView>();
    }
    private void Start()
    {
        juiceInventory = ship.GetComponent<JuiceInventory>();
        movementRange = Mathf.Abs(movementRange);
    }
    public void Move(float moveInput) // move cannon left and right based on input
    { // accepts moveInput as a float between -1 and 1
        Vector3 cameraRight = Camera.main.transform.right;
        transform.Translate(cameraRight * moveInput * movementSpeed * Time.deltaTime);
        Mathf.Clamp(transform.position.x, -movementRange, movementRange);
        transformView.Update();
    }
    public void Fire() // fire cannon, called by player input
    {
        // logic for firing cannon
        if(juiceInventory.juiceCount >= fireCost)
        {
            Vector3 spawn = new Vector3(transform.position.x, transform.position.y, transform.position.z + spawnOffset);
            PhotonNetwork.Instantiate(laser.name, spawn, Quaternion.identity);
            juiceInventory.juiceCount -= fireCost;
        }
    }
}