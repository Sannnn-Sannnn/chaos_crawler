using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Weapon
{
  public class Weapon : MonoBehaviour
  {

    private MouseListener _mousePositionListener;
    private SpriteRenderer _sprite;
    
    [Header("Objects")]
    [SerializeField] private InputActionReference mouseClickListener;
    [SerializeField] private GameObject bullet;
    
    [Header("Stats")]
    [SerializeField] private int damage = 0;
    [SerializeField] private int cooldown = 0;
    [SerializeField] private bool canHold = false;

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
    
    void Awake()
    {
      _sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
      var listener = GameObject.FindGameObjectWithTag("MouseListener");
      if (listener  != null) _mousePositionListener = listener.GetComponent<MouseListener>();
    }

    void Update()
    {
      Rotate();
    }

    private void Shoot()
    {
      var bulletInstance = Instantiate(bullet);
      Bullet bulletComponent = bulletInstance.GetComponent<Bullet>();
      // bulletInstance.transform.position = transform.position + transform.right;
      bulletInstance.transform.position = transform.position;
      bulletInstance.transform.rotation = transform.rotation;
      bulletComponent.SetDamage(damage);
      var direction = _mousePositionListener.GetAimDirection(transform.position);
      bulletComponent.SetDirection(direction);
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