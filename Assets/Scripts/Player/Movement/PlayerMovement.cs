using Dungeon;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private float speed = 5f;
  [SerializeField] private InputActionReference moveAction;

  private Rigidbody2D rb;
  private Vector2 movement;

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
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
    movement = moveAction.action.ReadValue<Vector2>();
    movement = movement.normalized;
  }

  private void FixedUpdate()
  {
    rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
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