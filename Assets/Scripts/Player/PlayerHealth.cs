using System;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
  public class PlayerHealth : MonoBehaviour
  {
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool isInmuneWhileDashing;

    private GameManager _gameManager;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
      currentHealth = maxHealth;
      _gameManager = FindAnyObjectByType<GameManager>();
      _playerMovement = GetComponent<PlayerMovement>();
    }

    public Vector2? Position()
    {
      if (currentHealth > 0)
      {
        return transform.position;
      }
        /*
         * Because the position (0, 0) is always at the spawn of the dungeon, there will never be any enemy that could
         * chase the player when in those coordinates -> a default/empty value cannot be returned instead
         */
        return null;
    }

    public void Restart()
    {
      currentHealth = maxHealth;
    }

    public void Damage(int damage)
    {
      var isDashing = _playerMovement.IsDashing();
      if (isDashing && isInmuneWhileDashing) return;
      currentHealth -= damage < currentHealth ? damage : currentHealth;
      if (currentHealth <= 0)
      {
        Die();
      }
    }

    public int MaxHealth()
    {
      return maxHealth;
    }

    public void IncreaseMaxHealth(int amount)
    {
      maxHealth += amount;
      currentHealth += amount;
    }
    
    public int Health()
    {
      return currentHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      var lifeRestore = other.GetComponent<LifeRestorationConsumable>();
      if (lifeRestore)
      {
        currentHealth = currentHealth + lifeRestore.HealthRestoration() > maxHealth ? maxHealth : currentHealth + lifeRestore.HealthRestoration();
        Destroy(other.gameObject);
      }
    }

    private void Die()
    {
      _gameManager.ShowDeath();
    }
  }
}