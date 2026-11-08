using Player;
using UnityEngine;
using Weapon;

namespace Enemy.Attack
{
  public class ShooterEnemy : MonoBehaviour
  {

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int damage;
    [SerializeField] private float fireRate;
    private float _lastAttack = 0;
    [SerializeField] private float initialWait;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float damageIncrement;

    private PlayerHealth _playerHealth;
    private GameManager _gameManager;
    private AudioManager _audioManager;

    private void Awake()
    {
      _gameManager = FindAnyObjectByType<GameManager>();
      var floor = _gameManager.CurrentFloor();

      damage += (floor - 1) * (int)damageIncrement;

      _playerHealth = GameObject.FindGameObjectWithTag("PlayerHitBox").GetComponent<PlayerHealth>();
      _lastAttack = Time.time + initialWait - fireRate;
      
      _audioManager = GameObject.FindWithTag("GameManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
      if (Time.time - _lastAttack > fireRate)
      {
        _lastAttack = Time.time;
        Attack();
      }
    }

    private void Attack()
    {
      var playerPosNullable = _playerHealth.Position();
      if (playerPosNullable != null)
      {
        var playerPos = playerPosNullable.Value;

        var bulletInstance = Instantiate(projectilePrefab);
        bulletInstance.transform.position = transform.position;

        var direction = ((Vector2)playerPos - (Vector2)transform.position).normalized;
        var rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        var bulletComponent = bulletInstance.GetComponent<EnemyBullet>();
        bulletComponent.SetSpeed(bulletSpeed);
        bulletComponent.SetDamage(damage);
        bulletComponent.SetDirection(direction);
        bulletComponent.SetRotation(rotation);
        
        _audioManager.PlaySfx(AudioManager.SoundId.EnemyAttack);
      }
    }
  }
}