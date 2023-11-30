using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    #region Variables
    public static GameUI Instance {  get; private set; }
    [Header("Blue Team")]
    [SerializeField] Slider blueHealth;
    [SerializeField] TextMeshProUGUI blueShipJuice;
    [Header("Red Team")]
    [SerializeField] Slider redHealth;
    [SerializeField] TextMeshProUGUI redShipJuice;
    #endregion
    #region Methods
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        // update the juice UI
        UpdateJuiceUI("Blue", 0);
        UpdateJuiceUI("Red", 0);
        InitializeBars();
    }
    private void InitializeBars()
    {
        // Get all ShipHealth components
        ShipHealth[] shipHealths = FindObjectsOfType<ShipHealth>();
        foreach (ShipHealth shipHealth in shipHealths)
        {
            // Get the team of the ship
            string team = shipHealth.team;
            // Get the health of the ship
            int health = shipHealth.GetHealth();
            // Update the health bar
            UpdateHealthUI(team, health, true);
        }
    }
    [PunRPC]
    public void UpdateJuiceUI(string _team = null, int _juice = -1)
    {
        if (_team == null || _juice == -1) return; // if no team or juice is specified, don't update anything
        if (_team == "Blue") blueShipJuice.text = _juice.ToString();
        else if (_team == "Red") redShipJuice.text = _juice.ToString();
    }
    [PunRPC]
    public void UpdateHealthUI(string _team = null, int _health = -1, bool _initializing = false)
    {
        if (_initializing)
        {
            if (_team == "Blue") blueHealth.maxValue = _health;
            else if (_team == "Red") redHealth.maxValue = _health;
            else Debug.Log("Invalid team specified for UpdateHeartUI().");
        }
        if (_health == -1) return;
        if (_team == "Blue") blueHealth.value = _health;
        else if (_team == "Red") redHealth.value = _health;
        else Debug.Log("Invalid team specified for UpdateHeartUI().");
    }
    #endregion
}