using Dungeon;
using Player;
using TMPro;
using UnityEngine;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private PlayerHealth _playerHealth;
    private DungeonGenerator _dungeonGenerator;

    private int _playerMaxHealth;
    
    void Awake()
    {
        _playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        _dungeonGenerator = GameObject.FindWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
        _playerMaxHealth = _playerHealth.MaxHealth();
    }

    void Update()
    {
        var playerHealth = _playerHealth.Health();
        var currentFloor = _dungeonGenerator.CurrentFloor();

        text.text = "HP: " + playerHealth + " / " + _playerMaxHealth + "\nFloor: " +  currentFloor;
    }
}
