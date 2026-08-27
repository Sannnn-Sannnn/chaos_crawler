using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Weapon
{
  public class Handgun : MonoBehaviour
  {

    private MouseListener _mousePositionListener;
    private SpriteRenderer _sprite;

    [Header("Objects")] [SerializeField] private InputActionReference mouseClickListener;
    [SerializeField] private GameObject bullet;

    [Header("Stats")] [SerializeField] private int damage = 0;
    [SerializeField] private float cooldown = 0f;
    private bool _isHolding = false;
    private float _lastShotTime = 0f;

    private void OnEnable()
    {
      mouseClickListener.action.Enable();
      mouseClickListener.action.started += OnAttackStarted;
        mouseClickListener.action.canceled += OnAttackCanceled;
    }

    private void OnDisable()
    {
      mouseClickListener.action.started -= OnAttackStarted;
      mouseClickListener.action.canceled -= OnAttackCanceled;
      mouseClickListener.action.Disable();
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
      _isHolding = true;
      Shoot();
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
      _isHolding = false;
    }

    void Awake()
    {
      _sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
      var listener = GameObject.FindGameObjectWithTag("MouseListener");
      if (listener != null) _mousePositionListener = listener.GetComponent<MouseListener>();
    }

    void Update()
    {
      Rotate();
      if (_isHolding) Shoot();
    }

    private void Shoot()
    {
      if (Time.time - _lastShotTime < cooldown) return;
      _lastShotTime = Time.time;
      var bulletInstance = Instantiate(bullet);
      Bullet bulletComponent = bulletInstance.GetComponent<Bullet>();
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
      _sprite.flipY = mouseDirection.x < 0? true : false;
    }
  }

}