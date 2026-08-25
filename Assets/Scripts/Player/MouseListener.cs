using UnityEngine;
using UnityEngine.InputSystem;

public class MouseListener : MonoBehaviour
{
  [SerializeField] private Camera mainCamera;
  [SerializeField] private RectTransform crosshair;
  [SerializeField] private InputActionReference mousePosition;

  private Vector2 mouseScreenPosition;
  private Vector2 mouseWorldPosition;

  public Vector2 MouseScreenPosition => mouseScreenPosition;
  public Vector2 MouseWorldPosition => mouseWorldPosition;
  
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
    return (mouseWorldPosition - origin).normalized;
  }

  private void Update()
  {
    mouseScreenPosition = mousePosition.action.ReadValue<Vector2>();
    Vector3 screenPosition = new Vector3(
      mouseScreenPosition.x,
      mouseScreenPosition.y,
      -mainCamera.transform.position.z
    );

    mouseWorldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
    
    crosshair.position = mouseWorldPosition;
  }
}