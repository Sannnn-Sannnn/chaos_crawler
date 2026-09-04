using UnityEngine;

namespace Weapon
{
  public class FireworkBullet : MonoBehaviour, IBullet
  {
    private Vector2 _direction = Vector2.zero;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float pierceCount = 1;

    [SerializeField] private GameObject spark;
    [SerializeField] private int sparkDamage = 5;
    [SerializeField] private int sparkCount = 10;
    [SerializeField] private float sparkLifetime = 5f;
    [SerializeField] private float sparkLifetimeDeviation = 5f;
    [SerializeField] private float sparkSpeed = 20f;
    [SerializeField] private float sparkSpeedDeviation = 5f;
    
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
        Explode();
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

    private void Explode()
    {
      for (int i = 0; i < sparkCount; i++)
      {
        var sparkInstance = Instantiate(spark);
        Spark sparkComponent = sparkInstance.GetComponent<Spark>();
        sparkInstance.transform.position = transform.position;
        sparkComponent.SetDamage(sparkDamage);
        
        // Generate random direction to each spark
        var x = Random.Range(-1f, 1f);
        var y = Random.Range(-1f, 1f);
        var direction =  new Vector3(x, y, transform.position.z).normalized;
        sparkComponent.SetDirection(direction);
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sparkInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
        sparkComponent.SetSpeed(sparkSpeed + Random.Range(-sparkSpeedDeviation, sparkSpeedDeviation));
        sparkComponent.SetLifetime(sparkLifetime + Random.Range(-sparkLifetimeDeviation,  sparkLifetimeDeviation));
      }
      Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
      //private void OnCollisionEnter2D(Collision2D collision)
    {
      EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
      if (enemy != null)
      {
        enemy.Damage(_damage);
        pierceCount--;
        if (pierceCount == 0) Explode();
      }
      else
      {
        Explode(); 
      }
    }
  }
}