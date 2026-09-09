using System;
using Player;
using UnityEngine;

namespace Enemy
{
  public class ChaserEnemy : MonoBehaviour
  {

    private PlayerHealth _player;
    private Rigidbody2D _rb;
    [SerializeField] private float speed;

    private void Awake()
    {
      _player = GameObject.FindWithTag("PlayerHitBox").GetComponent<PlayerHealth>();
      _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
      Move();
    }

    private void Move()
    {
      var playerPos = _player.Position();
      if (playerPos != null)
      {
        Vector2 direction = (playerPos.Value - (Vector2)transform.position).normalized;
        Vector2 newPos = _rb.position + direction * speed * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);
      }
    }
    
  }
}