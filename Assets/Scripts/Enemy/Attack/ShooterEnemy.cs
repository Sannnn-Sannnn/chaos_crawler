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
    [SerializeField] private float bulletSpeed;

    private PlayerHealth _playerHealth;

    private void Awake()
    {
      _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
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
        bulletInstance.transform.rotation = Quaternion.Euler(0f, 0f,
          Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        var bulletComponent = bulletInstance.GetComponent<EnemyAttack>();
        bulletComponent.SetSpeed(bulletSpeed);
        bulletComponent.SetDamage(damage);
        bulletComponent.SetDirection(direction);
      }
    }
  }
}