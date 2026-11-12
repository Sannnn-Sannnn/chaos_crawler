using UnityEngine;

namespace Weapon
{
  public class Handgun : WeaponBase
  {
    [Header("Bullet Setup")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Stats")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageIncrement = 2f;
    [SerializeField] private float cooldown = 0.2f;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float deviation = 0f;
    
    [SerializeField] private float cooldownDeviation = 0.02f;

    [Header("Audio")] [SerializeField] private int playSfxEveryXShots = 1;

    private int _shotCounter;
    
    private GameManager _gameManager;
    private AudioManager _audioManager;

    protected override void Awake()
    {
      base.Awake();
      
      _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
      var floow = _gameManager.CurrentFloor();
      damage += (floow - 1) * (int)damageIncrement;

      cooldown += Random.Range(-cooldownDeviation, cooldownDeviation);
      
      _audioManager = GameObject.FindWithTag("GameManager").GetComponent<AudioManager>();
      _shotCounter = playSfxEveryXShots;
    }

    public override void Shoot(Vector2 aimDirection)
    {
      // Validar si está equipada y si ya pasó el cooldown
      if (!IsEquipped() || Time.time - LastShotTime < cooldown) return;
      PlaySound();
      LastShotTime = Time.time;

      // Determinar posición de spawn (firePoint o la posición del arma)
      Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

      // Instanciar el proyectil con la rotación actual del arma
      var bulletInstance = Instantiate(bulletPrefab, spawnPos, transform.rotation);

      // Configurar los parámetros del proyectil
      if (bulletInstance.TryGetComponent<IBullet>(out var bulletComponent))
      {
        bulletComponent.SetDamage(damage);
        bulletComponent.SetSpeed(speed);
        
        float randomAngle = Random.Range(-deviation, deviation);
        Quaternion randomDeviation = Quaternion.Euler(0f, 0f, randomAngle);
        
        bulletComponent.SetDirection((randomDeviation * aimDirection).normalized);
      }
      Use();
    }

    private void PlaySound()
    {
      if (Time.time - LastShotTime >= cooldown * playSfxEveryXShots) _shotCounter = 0;
      if (_shotCounter % playSfxEveryXShots == 0) _audioManager.PlaySfx(AudioManager.SoundId.PlayerAttack);
      _shotCounter++;
    }
  }
}