using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class Cannon : MonoBehaviour
{
    PhotonView view;
    [Tooltip("The Cannon Object this control panel Moves")]
    [SerializeField] GameObject cannon;
    [Tooltip("The Ship Juice Inventory this cannon is attached to.")]
    [SerializeField] GameObject shipInventory;
    [Tooltip("The laser prefab to be instantiated when the cannon fires.")]
    [SerializeField] GameObject laser;
    [Tooltip("How far in front of the cannon to spawn the laser.")]
    [SerializeField] float spawnOffset = 1f;
    [SerializeField] int fireCost = 15;
    [Tooltip("How far left or right the cannon can move as a float. Negative values will be converted to positive.")]
    [SerializeField] float movementRange = 10f;
    [Tooltip("How fast the cannon moves as a float")]
    [SerializeField] float movementSpeed = 2f;
    public bool inUse;
    public float moveInput;
    JuiceInventory juiceInventory;
    private void Awake()
    {
        view = gameObject.GetComponent<PhotonView>();
    }
    private void Start()
    {
        juiceInventory = shipInventory.GetComponent<JuiceInventory>();
        movementRange = Mathf.Abs(movementRange);
    }
    private void FixedUpdate()
    {
        Move();
    }
    public void Move() // move cannon left and right based on input
    { // accepts moveInput as a float between -1 and 1
        cannon.transform.Translate(Vector3.right * moveInput * movementSpeed * Time.deltaTime);
        Mathf.Clamp(transform.position.x, -movementRange, movementRange);
    }
    public void Fire() // fire cannon, called by player input
    {
        // logic for firing cannon
        if(juiceInventory.juiceCount >= fireCost)
        {
            Vector3 spawn = cannon.transform.position + Vector3.forward * spawnOffset;
            PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", laser.name), spawn, Quaternion.identity);
            juiceInventory.juiceCount -= fireCost;
        }
    }
}