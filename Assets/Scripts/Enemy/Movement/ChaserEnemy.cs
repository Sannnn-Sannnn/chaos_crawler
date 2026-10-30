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

    [SerializeField] private bool isLookingRight = true;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
      _player = GameObject.FindWithTag("PlayerHitBox").GetComponent<PlayerHealth>();
      _rb = GetComponent<Rigidbody2D>();

      _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

        FlipSprite(direction.x);
      }
    }

    private void FlipSprite(float x)
    {
      if (x > 0 && !isLookingRight)
      {
        isLookingRight = true;
        _spriteRenderer.flipX = false;
      }
      else if (x < 0 && isLookingRight)
      {
        isLookingRight = false;
        _spriteRenderer.flipX = true;
      }
    }
  }
}