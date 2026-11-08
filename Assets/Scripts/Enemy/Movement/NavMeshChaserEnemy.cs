using Player;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshChaserEnemy : MonoBehaviour
{
  [SerializeField] private float speed = 3f;
  [SerializeField] private bool isLookingRight = true;
 
  private Transform _player;
  private NavMeshAgent _agent;
  private SpriteRenderer _spriteRenderer;

  private Vector3 _currentPosition;
  private Vector3 _lastPosition;

  private void Awake()
  {
    _player = GameObject.FindGameObjectWithTag("Player").transform;
    _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    _agent = GetComponent<NavMeshAgent>();
    _agent.updateRotation = false;
    _agent.updateUpAxis = false;
    _agent.speed = speed;
    
    _currentPosition = transform.position;
    _lastPosition = transform.position;
  }

  private void Update()
  {
    Move();
  }

  private void Move()
  {
    _agent.SetDestination(_player.position);
    _lastPosition = _currentPosition;
    _currentPosition = transform.position;
    FlipSprite();
  }

  private void FlipSprite()
  {
    var xDirection = _currentPosition.x - _lastPosition.x;
    if (xDirection < 0 && isLookingRight)
    {
      _spriteRenderer.flipX = true;
      isLookingRight = false;
    }
    else if  (xDirection > 0 && !isLookingRight)
    {
      _spriteRenderer.flipX = false;
      isLookingRight = true;
    }
  }
}