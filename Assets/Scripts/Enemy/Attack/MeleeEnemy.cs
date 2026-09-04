using Player;
using UnityEngine;
using Weapon;

namespace Enemy.Attack
{
  public class MeleeEnemy : MonoBehaviour
  {
    
    [SerializeField] private GameObject attackprefab;
    [SerializeField] private int damage;
    [SerializeField] private float cooldown;
    private float _lastAttack = 0;

    private PlayerHealth _playerHealth;

    private void Awake()
    {
      _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
      _lastAttack = 0;
    }

    private void Update()
    {
      if (Time.time - _lastAttack > cooldown)
      {
        _lastAttack = Time.time;
        Attack();
      }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
      if (Time.time - _lastAttack > cooldown)
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

        var bulletInstance = Instantiate(attackprefab);
        bulletInstance.transform.position = transform.position;

        var bulletComponent = bulletInstance.GetComponent<EnemyAttack>();
        bulletComponent.SetSpeed(0);
        bulletComponent.SetDamage(damage);
      }
    }
  }
}