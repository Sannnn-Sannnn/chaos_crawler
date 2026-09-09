using Player;
using UnityEngine;
using Weapon;

public class EnemyMeleeAttack : MonoBehaviour
{
  [SerializeField] private float lifetime = 5f;
  private float _birthTime;
  private int _damage = 0;
  private Rigidbody2D _rb;

  private bool _hasHit = false;

  private void Awake()
  {
    _birthTime = Time.time;
    _rb = GetComponent<Rigidbody2D>();
  }

  private void FixedUpdate()
  {
    if (Time.time - _birthTime > lifetime)
    {
      Destroy(gameObject);
    }
  }

  public void SetRotation(Quaternion rotation)
  {
    transform.rotation = rotation;
  }

  public void SetDamage(int damage)
  {
    _damage = damage;
  }

  public void SetLifetime(float newLifetime)
  {
    lifetime = newLifetime;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (_hasHit) return;
    var player = collision.gameObject.GetComponent<PlayerHealth>();
    // Collided with player
    if (player != null)
    {
      player.Damage(_damage);
      _hasHit = true;
      Destroy(gameObject,  lifetime);
    }
  }
}