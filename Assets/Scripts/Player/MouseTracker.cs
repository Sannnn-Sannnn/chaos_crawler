using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseListener : MonoBehaviour
{
  public event Action<int> OnWheelScroll;
  
  [SerializeField] private Camera mainCamera;
  [SerializeField] private RectTransform crosshair;
  [SerializeField] private InputActionReference mousePosition;
  [SerializeField] private InputActionReference mouseScroll;
  
  [SerializeField] private float mouseScrollCooldown;

  [SerializeField] private bool inverted;
   private float _mouseScrollStart;

  private Vector2 _mouseScreenPosition;
  private Vector2 _mouseWorldPosition;

  public Vector2 MouseScreenPosition => _mouseScreenPosition;
  public Vector2 MouseWorldPosition => _mouseWorldPosition;

  private void OnEnable()
  {
    mousePosition.action.Enable();
    mouseScroll.action.Enable();
    mouseScroll.action.performed += OnScroll;
  }

  private void OnDisable()
  {
    mousePosition.action.Disable();
    mouseScroll.action.Disable();
    mousePosition.action.performed -= OnScroll;
  }

  public Vector2 GetAimDirection(Vector2 origin)
  {
    return (_mouseWorldPosition - origin).normalized;
  }

  private void Update()
  {
    _mouseScreenPosition = mousePosition.action.ReadValue<Vector2>();
    Vector3 screenPosition = new Vector3(
      _mouseScreenPosition.x,
      _mouseScreenPosition.y,
      -mainCamera.transform.position.z
    );

    _mouseWorldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

    crosshair.position = _mouseWorldPosition;
  }
  
  private void OnScroll(InputAction.CallbackContext context)
  {
    Vector2 scrollValue = context.ReadValue<Vector2>();
    var inversion = inverted ? -1 : 1;

    if (Time.time - _mouseScrollStart > mouseScrollCooldown)
    {
      _mouseScrollStart = Time.time;
      if (scrollValue.y > 0) OnWheelScroll?.Invoke(-1 * inversion);
      else if (scrollValue.y < 0) OnWheelScroll?.Invoke(1 * inversion);
    }
  }
}