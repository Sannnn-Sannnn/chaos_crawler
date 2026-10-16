using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseListener : MonoBehaviour
{
  public event Action<int> OnWheelScroll;

  [SerializeField] private Camera mainCamera;
  [SerializeField] private RectTransform crosshair;
  [SerializeField] private InputActionReference mousePosition;

  private Vector2 _mouseScreenPosition;
  private Vector2 _mouseWorldPosition;

  public Vector2 MouseScreenPosition => _mouseScreenPosition;
  public Vector2 MouseWorldPosition => _mouseWorldPosition;

  private void OnEnable()
  {
    mousePosition.action.Enable();
  }

  private void OnDisable()
  {
    mousePosition.action.Disable();
  }

  public Vector2 GetAimDirection(Vector2 origin)
  {
    return (_mouseWorldPosition - origin).normalized;
  }

  private void FixedUpdate()
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
}