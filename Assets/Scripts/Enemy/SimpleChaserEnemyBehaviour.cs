using System;
using UnityEngine;

namespace Enemy
{
  public class SimpleChaserEnemyBehaviour : MonoBehaviour
  {

    private GameObject _player;
    private Rigidbody2D _rb;
    [SerializeField] private float speed;

    private void Awake()
    {
      _player = GameObject.FindWithTag("Player");
      _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
      if (_player == null) return;
      Move();
    }

    private void Move()
    {
      Vector3 playerPos = _player.transform.position;
      Vector2 direction = (playerPos - transform.position).normalized;
      Vector2 newPos = _rb.position + direction * speed * Time.fixedDeltaTime;
      _rb.MovePosition(newPos);
    }
    
  }
}