using UnityEngine;

namespace Player.WeaponSet
{
  public class WeaponSet : MonoBehaviour
  {
    private MouseListener _mousePositionListener;

    void Awake()
    {
      var listener = GameObject.FindGameObjectWithTag("MouseListener");
      if (listener != null) _mousePositionListener = listener.GetComponent<MouseListener>();
    }
    
    private void Update()
    {
      Rotate();
    }
    
    private void Rotate()
    {
      // Sprite rotation
      if (_mousePositionListener == null) return;
      var mouseDirection = _mousePositionListener.GetAimDirection(transform.position);
      var angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(0f, 0f, angle);

      // Sprite flipation
      //transform. = mouseDirection.x < 0 ? true : false;
    }
  }

}