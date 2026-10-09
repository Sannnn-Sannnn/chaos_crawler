using Dungeon;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI floorNumber;
    [SerializeField] private TextMeshProUGUI lifeIndicator;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;

    private PlayerHealth _playerHealth;
    private GameManager _gameManager;
    private PlayerMovement _playerMovement;

    private int _playerMaxHealth;
    private int _playerMaxStamina;

    void Awake()
    {
        _playerHealth = GameObject.FindWithTag("PlayerHitBox").GetComponent<PlayerHealth>();
        _playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _playerMaxHealth = _playerHealth.MaxHealth();
        _playerMaxStamina = _playerMovement.MaxStamina();
    }

    void Update()
    {
        var currentFloor = _gameManager.CurrentFloor();
        var playerHealth = _playerHealth.Health();
        var playerStamina = _playerMovement.Stamina();

        floorNumber.text = currentFloor.ToString();
        lifeIndicator.text = "HP: " + playerHealth + "/" + _playerMaxHealth;
        healthBar.fillAmount = (float)playerHealth / _playerMaxHealth;
        staminaBar.fillAmount = (float)playerStamina / _playerMaxStamina;
    }
}
