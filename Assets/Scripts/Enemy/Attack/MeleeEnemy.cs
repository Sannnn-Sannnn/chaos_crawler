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
    [SerializeField] private float damageIncrement;
    private float _lastAttack = 0;

    private PlayerHealth _playerHealth;
    private GameManager _gameManager;


    private void Awake()
    {
      _gameManager = FindAnyObjectByType<GameManager>();
      var floor = _gameManager.CurrentFloor();

      damage += (floor - 1) * (int)damageIncrement;
      _playerHealth = GameObject.FindGameObjectWithTag("PlayerHitBox").GetComponent<PlayerHealth>();
      _lastAttack = 0;
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
      var player = collider2D.gameObject.GetComponent<PlayerHealth>();
      if (player != null && Time.time - _lastAttack > cooldown)
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

        var attackInstance = Instantiate(attackprefab);
        attackInstance.transform.position = playerPos;

        //var direction = (playerPos - (Vector2)transform.position).normalized;
        //var rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        
        var bulletComponent = attackInstance.GetComponent<EnemyMeleeAttack>();
        bulletComponent.SetDamage(damage);
        //bulletComponent.SetRotation(rotation);
      }
    }
  }
}