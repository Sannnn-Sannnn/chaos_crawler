using Dungeon;
using Player;
using TMPro;
using UnityEngine;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
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

        text.text = "HP: " + playerHealth + " / " + _playerMaxHealth + "\nFloor: " +  currentFloor;
    }
}
