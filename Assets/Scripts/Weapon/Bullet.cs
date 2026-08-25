using UnityEngine;

namespace Weapon
{
  public class Bullet : MonoBehaviour
  {
    private Vector2 _direction = Vector2.zero;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float speed = 10f;
    private float _birthTime;
    private Rigidbody2D _rb;

    private void Awake()
    {
      _birthTime = Time.time;
      _rb =  GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
      var currentTime = Time.time;
      if (currentTime - _birthTime > lifetime)
      {
        Destroy(gameObject);
      }
      Vector2 newPosition = _rb.position + _direction * speed * Time.deltaTime;
      _rb.MovePosition(newPosition);
    }

    public void SetDirection(Vector3 direction)
    {
      _direction = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      if (!collision.gameObject.CompareTag("Player"))
      {
        Destroy(gameObject);
      }
    }
  }
}