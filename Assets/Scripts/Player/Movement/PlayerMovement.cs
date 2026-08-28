using Dungeon;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private float speed = 5f;
  [SerializeField] private InputActionReference moveAction;
  private bool _isAlive = true;

  private Rigidbody2D _rb;
  private Vector2 _movement;

  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
  }

  private void OnEnable()
  {
    moveAction.action.Enable();
  }

  private void OnDisable()
  {
    moveAction.action.Disable();
  }

  private void Update()
  {
    _movement = moveAction.action.ReadValue<Vector2>();
    _movement = _movement.normalized;
  }

  private void FixedUpdate()
  {
    if (_isAlive)
    {
      _rb.MovePosition(_rb.position + _movement * speed * Time.fixedDeltaTime); 
    }
  }
  
  public void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Flag"))
    {
      var flag = other.gameObject.GetComponent<Flag>();
      flag.Reset();
    }
  }
}