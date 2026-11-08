using UnityEngine;
using UnityEngine.InputSystem;

public class BasicPlayerMovement : MonoBehaviour
{
  [Header("Basic movement")]
  [SerializeField] private float speed = 5f;
  [SerializeField] private InputActionReference moveAction;

  private Rigidbody2D _rb;
  private Vector2 _movement;

  private void Awake() => _rb = GetComponent<Rigidbody2D>();

  private void OnEnable() => moveAction.action.Enable();

  private void OnDisable() => moveAction.action.Disable();

  private void FixedUpdate()
  {
      _movement = moveAction.action.ReadValue<Vector2>().normalized;
      _rb.MovePosition(_rb.position + _movement * speed * Time.fixedDeltaTime);
  }
}