using System.Collections;
using Dungeon;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  
  private bool _isAlive = true;
  
  [Header("Basic movement")]
  [SerializeField] private float speed = 5f;
  [SerializeField] private InputActionReference moveAction;
  
  [Header("Dash")]
  [SerializeField] private InputActionReference dashAction;
  [SerializeField] private float dashSpeed;
  [SerializeField] private float dashDuration;

  private int _maxStamina = 100;
  [SerializeField] private float _stamina;
  [SerializeField] private float staminaRecovery;
  
  private bool _isDashing = false;
  private bool _canDash = true;

  private Rigidbody2D _rb;
  private Vector2 _movement;
  private Vector2 _dashDirection;

  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
    _stamina = _maxStamina;
  }

  private void OnEnable()
  {
    moveAction.action.Enable();
    dashAction.action.Enable();
    dashAction.action.performed += HandleDash;
  }

  private void OnDisable()
  {
    moveAction.action.Disable();
    dashAction.action.Disable();
    dashAction.action.performed -= HandleDash;
  }

  private void Update()
  {
    if (_isDashing || !_isAlive) return;
    _movement = moveAction.action.ReadValue<Vector2>().normalized;
    _dashDirection = _movement == Vector2.zero ? _dashDirection : _movement;
  }

  private void FixedUpdate()
  {
    if (!_isAlive) return;
    if (!_isDashing)
    {
      _rb.MovePosition(_rb.position + _movement * speed * Time.fixedDeltaTime);
      if (_stamina < _maxStamina)
      {
        _stamina += staminaRecovery;
      }
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

  private void HandleDash(InputAction.CallbackContext ctx)
  {
    if (_isDashing || !_isAlive) return;
    if (_stamina < _maxStamina / 3) return;
    StartCoroutine(DashRoutine());
  }

  private IEnumerator DashRoutine()
  {
    _isDashing = true;
    _stamina -= _maxStamina / 3;

    float elapsedTime = 0f;
    
    while (elapsedTime < dashDuration)
    {
      _rb.linearVelocity = _dashDirection.normalized * dashSpeed; 
      elapsedTime += Time.fixedDeltaTime;
      yield return new WaitForFixedUpdate();
    }
    _rb.linearVelocity = Vector2.zero;
    _isDashing = false;
  }

  public int MaxStamina() => _maxStamina;
  public float Stamina() => _stamina;
  public bool IsDashing() => _isDashing;
}