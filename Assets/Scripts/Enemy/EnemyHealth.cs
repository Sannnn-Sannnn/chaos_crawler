using System;
using Dungeon;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private int healthIncrement;
    [SerializeField] private TextMeshPro hpIndicator;
    private EnemyDropBehaviour _dropper;
    private EnemySpawner _spawner;
    
    private GameManager _gameManager;
    private AudioManager _audioManager;
    
    void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        var floor = _gameManager.CurrentFloor();

        maxHealth += (floor - 1) * healthIncrement;
        health = maxHealth;
        _dropper = GetComponent<EnemyDropBehaviour>();
        
        hpIndicator =  GetComponentInChildren<TextMeshPro>();
        
        _audioManager = GameObject.FindWithTag("GameManager").GetComponent<AudioManager>();
    }
    
    void Update()
    {
        hpIndicator.text = health.ToString();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        _spawner = spawner;
    }

    public void Damage(int damage)
    {
        health -= damage;
        _audioManager.PlaySfx(AudioManager.SoundId.EnemyDamage);
    }

    private void OnDestroy()
    {
        Die();
    }

    private void Die()
    {
        _gameManager.Death();
        _spawner.Down();
        _dropper.Drop();
        Destroy(gameObject);
        _audioManager.PlaySfx(AudioManager.SoundId.EnemyDeath);
    }
    
}
