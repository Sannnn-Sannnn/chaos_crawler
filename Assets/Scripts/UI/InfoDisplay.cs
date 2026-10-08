using Dungeon;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI floorNumber;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    
    private PlayerHealth _playerHealth;
    private GameManager _gameManager;

    private int _playerMaxHealth;
    
    void Awake()
    {
        _playerHealth = GameObject.FindWithTag("PlayerHitBox").GetComponent<PlayerHealth>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _playerMaxHealth = _playerHealth.MaxHealth();
    }

    void Update()
    {
        var playerHealth = _playerHealth.Health();
        var currentFloor = _gameManager.CurrentFloor();

        floorNumber.text = currentFloor.ToString();
        healthBar.fillAmount = (float) playerHealth / _playerMaxHealth;
    }
}
