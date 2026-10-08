using UnityEngine;

namespace Weapon
{
  public class EnermgyBullet : MonoBehaviour, IBullet
  {
    private Vector2 _direction = Vector2.zero;
    [SerializeField] private float lifetime = 5f;
    private float _speed = 1f;
    private float _birthTime;
    private int _damage = 0;
    private Rigidbody2D _rb;

    private void Awake()
    {
      _birthTime = Time.time;
      _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
      var currentTime = Time.time;
      if (currentTime - _birthTime > lifetime)
      {
        Destroy(gameObject);
      }
      Vector2 newPosition = _rb.position + _direction * _speed * Time.deltaTime;
      _rb.MovePosition(newPosition);
    }

    public void SetDirection(Vector3 direction)
    {
      _direction = direction;
    }

    public void SetDamage(int damage)
    {
      _damage = damage;
    }

    public void SetSpeed(float newSpeed)
    {
      _speed = newSpeed;
    }

    public void SetLifetime(float newLifetime)
    {
      lifetime = newLifetime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    //private void OnCollisionEnter2D(Collision2D collision)
    {
      var enemy = collision.gameObject.GetComponent<EnemyHealth>();
      if (enemy != null)
      {
        enemy.Damage(_damage);
      }
    }
  }
}