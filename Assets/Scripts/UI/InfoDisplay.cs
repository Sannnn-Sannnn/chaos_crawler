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
    

    void Awake()
    {
        _playerHealth = GameObject.FindWithTag("PlayerHitBox").GetComponent<PlayerHealth>();
        _playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        var currentFloor = _gameManager.CurrentFloor();
        
        var playerMaxHealth = _playerHealth.MaxHealth();
        var playerHealth = _playerHealth.Health();
        
        var playerMaxStamina = _playerMovement.MaxStamina();
        var playerStamina = _playerMovement.Stamina();

        floorNumber.text = currentFloor.ToString();
        lifeIndicator.text = "HP: " + playerHealth + "/" + playerMaxHealth;
        healthBar.fillAmount = (float)playerHealth / playerMaxHealth;
        staminaBar.fillAmount = (float)playerStamina / playerMaxStamina;
    }
}
