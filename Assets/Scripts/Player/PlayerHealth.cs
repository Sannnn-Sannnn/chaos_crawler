using System.Collections;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
  public class PlayerHealth : MonoBehaviour
  {
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool isInmuneWhileDashing;
    [SerializeField] private float immunityTime;
    private bool _inImmunityTime = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField, Range(0f, 1f)] private float blinkAlpha = 0.3f;
    private Coroutine _blinkRoutine;
    
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private PlayerMovement _playerMovement;

    [SerializeField] private Animator animator;
    [SerializeField] private float deathAnimationDuration;

    [SerializeField] private GameObject weapons;

    private void Awake()
    {
      currentHealth = maxHealth;
      _gameManager = FindAnyObjectByType<GameManager>();
      _playerMovement = GetComponentInParent<PlayerMovement>();
      _audioManager = GameObject.FindWithTag("GameManager").GetComponent<AudioManager>();
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
      if (_inImmunityTime) return;
      currentHealth -= damage < currentHealth ? damage : currentHealth;
      if (currentHealth <= 0)
      {
        StopBlink();
        StartCoroutine(Die());
      }
      else
      {
        StopBlink();
        _blinkRoutine = StartCoroutine(ReceiveDamage());
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
      if (lifeRestore && currentHealth > 0)
      {
        currentHealth = currentHealth + lifeRestore.HealthRestoration() > maxHealth ? maxHealth : currentHealth + lifeRestore.HealthRestoration();
        _audioManager.PlaySfx(AudioManager.SoundId.Consumable);
        Destroy(other.gameObject);
      }
    }

    private IEnumerator Die()
    {
      _audioManager.PlaySfx(AudioManager.SoundId.PlayerDeath);
      animator.SetBool("isAlive", false);
      weapons.SetActive(false);
      yield return new WaitForSeconds(deathAnimationDuration);
      _gameManager.ShowDeath();
    }

    private IEnumerator ReceiveDamage()
    {
      _audioManager.PlaySfx(AudioManager.SoundId.PlayerDamage);
      var elapsed = 0f;
      var faded = true;
      _inImmunityTime = true;
      while (elapsed < immunityTime)
      {
        SetAlpha(faded ? blinkAlpha : 1f);
        faded = !faded;
        yield return new WaitForSeconds(blinkInterval);
        elapsed += blinkInterval;
      }

      SetAlpha(1f);
      _blinkRoutine = null;
      _inImmunityTime = false;
    }
    
    private void StopBlink()
    {
      if (_blinkRoutine != null)
      {
        StopCoroutine(_blinkRoutine);
        _blinkRoutine = null;
      }
      SetAlpha(1f);
    }

    private void SetAlpha(float alpha)
    {
      var c = spriteRenderer.color;
      c.a = alpha;
      spriteRenderer.color = c;
    }
  }
}