using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Weapon
{
  public class Weapon : MonoBehaviour
  {

    private MouseListener _mousePositionListener;
    private SpriteRenderer _sprite;
    [SerializeField] private InputActionReference mouseClickListener;
    [SerializeField] private GameObject bullet;

    private void OnEnable()
    {
      mouseClickListener.action.Enable();
      mouseClickListener.action.performed += OnAttack;
    }

    private void OnDisable()
    {
      mouseClickListener.action.Disable();
      mouseClickListener.action.performed -= OnAttack;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
      Shoot();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
      _sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
      var listener = GameObject.FindGameObjectWithTag("MouseListener");
      if (listener  != null) _mousePositionListener = listener.GetComponent<MouseListener>();
    }

    // Update is called once per frame
    void Update()
    {
      Rotate();
    }

    private void Shoot()
    {
      var bulletInstance = Instantiate(bullet);
      bulletInstance.transform.position = transform.position + transform.right;
      bulletInstance.transform.rotation = transform.rotation;
      var direction = _mousePositionListener.GetAimDirection(transform.position);
      bulletInstance.GetComponent<Bullet>().SetDirection(direction);
    }   

    private void Rotate()
    {
      // Sprite rotation
      if (_mousePositionListener == null) return;
      var mouseDirection = _mousePositionListener.GetAimDirection(transform.position);
      var angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(0f, 0f, angle);
      
      
      // Sprite flipation
      if (mouseDirection.x < 0)
      {
        _sprite.flipY = true;
      }
      else
      {
        _sprite.flipY = false;
      }
    }
  }
   
}