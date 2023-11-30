using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Collections;

public class Cannon : MonoBehaviour
{
    PhotonView view;
    [Tooltip("The Cannon Object this control panel Moves")]
    [SerializeField] GameObject cannon;
    [Tooltip("The Shield Object this cannon controls")]
    [SerializeField] GameObject shields;
    [Tooltip("Time in seconds between each intance of shields consuming juice")]
    [SerializeField] float shieldDrainRateTime = 1f;
    [Tooltip("The Ship Juice Inventory this cannon is attached to.")]
    [SerializeField] JuiceInventory shipJuiceInventory; // assigned in inspector -Leeman
    [Tooltip("The laser prefab to be instantiated when the cannon fires.")]
    [SerializeField] GameObject laser;
    [Tooltip("How far in front of the cannon to spawn the laser.")]
    [SerializeField] float spawnOffset = 1f;
    [SerializeField] public int fireCost = 15;
    [Tooltip("How far left or right the cannon can move as a float. Negative values will be converted to positive.")]
    [SerializeField] float movementRange = 10f;
    [Tooltip("How fast the cannon moves as a float")]
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] bool blueTeam;
    public bool inUse;
    public float moveInput;
    private void Awake()
    {
        view = gameObject.GetComponent<PhotonView>();
    }
    private void Start()
    {
        movementRange = Mathf.Abs(movementRange);
        if (!blueTeam)
        {
            movementSpeed *= -1;
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    public void Move() // move cannon left and right based on input
    { // accepts moveInput as a float between -1 and 1
        cannon.transform.position = new Vector3(Mathf.Clamp(cannon.transform.position.x + (moveInput * movementSpeed * Time.deltaTime), -movementRange, movementRange), cannon.transform.position.y, cannon.transform.position.z);
    }
    [PunRPC]
    public void Fire() // fire cannon, called by player input
    {
        // logic for firing cannon
        if(shipJuiceInventory.juiceCount >= fireCost)
        {
            Vector3 spawn = cannon.transform.position + cannon.transform.forward * spawnOffset;
            Quaternion spawnRotation = Quaternion.identity;
            if (blueTeam)
            {
                spawnRotation = Quaternion.Euler(90, 0, 0);
            }
            else
            {
                spawnRotation = Quaternion.Euler(-90, 0, 0);
            }
            PhotonNetwork.Instantiate(Path.Combine("Spawn Objects", laser.name), spawn, spawnRotation);
            shipJuiceInventory.gameObject.GetComponent<PhotonView>().RPC("AcceptJuice", RpcTarget.All, -fireCost);
        }
    }
    [PunRPC]
    public void FlipOccupiedBool()
    {
        inUse = !inUse;
        if(shipJuiceInventory.juiceCount > 0)
        {
            shields.SetActive(inUse);
            StartCoroutine(ShieldDrain());
        }
    }
    IEnumerator ShieldDrain()
    { 
        while (inUse)
        {
            yield return new WaitForSeconds(shieldDrainRateTime);
            if (shipJuiceInventory.juiceCount > 0)
            {
                shipJuiceInventory.gameObject.GetComponent<PhotonView>().RPC("AcceptJuice", RpcTarget.All, -1);
            }
            else
            {
                StopCoroutine("ShieldDrain");
                shields.SetActive(false);
            }
        }
    }
}