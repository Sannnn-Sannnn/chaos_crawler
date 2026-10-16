using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponChangeTracker : MonoBehaviour
{
  public event Action<int> OnWheelScroll;
  public event Action<int> OnNumberPressed;
  
  [SerializeField] private InputActionReference mouseScroll;
  [SerializeField] private float mouseScrollCooldown;
  [SerializeField] private bool inverted;
  [SerializeField] private InputActionReference numpad;
  private float _mouseScrollStart;
  
  private void OnEnable()
  {
    mouseScroll.action.Enable();
    mouseScroll.action.performed += OnScroll;
    
    numpad.action.Enable();
    numpad.action.performed += OnPressed;
  }

  private void OnDisable()
  {
    mouseScroll.action.Disable();
    mouseScroll.action.performed -= OnScroll;
    
    numpad.action.Disable();
    numpad.action.performed -= OnPressed;
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

  private void OnPressed(InputAction.CallbackContext context)
  {
    string controlName = context.control.name;
    int selectedSlot = -1;
    
    if (controlName.Contains("1")) selectedSlot = 0;
    else if (controlName.Contains("2")) selectedSlot = 1;
    else if (controlName.Contains("3")) selectedSlot = 2;
    else if (controlName.Contains("4")) selectedSlot = 3;
    
    if (selectedSlot != -1)
    {
      OnNumberPressed?.Invoke(selectedSlot);
    }
  }
}