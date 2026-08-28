using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapon
{
  public class Shotgun : MonoBehaviour
  {

    private MouseListener _mousePositionListener;
    private SpriteRenderer _sprite;

    [Header("Objects")] [SerializeField] private InputActionReference mouseClickListener;
    [SerializeField] private GameObject bullet;

    [Header("Stats")] [SerializeField] private int damage = 0;
    [SerializeField] private float cooldown = 0f;
    [SerializeField] private int bulletCount = 0;
    [SerializeField] private float shotAngleDeviation = 0f;
    [SerializeField] private float speed = 0f;
    [SerializeField] private float bulletSpeedDeviation = 0f;
    private bool _isHolding = false;
    private float _lastShotTime = -1000f;

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
      var weaponDirection = _mousePositionListener.GetAimDirection(transform.position);
      for (int i = 0; i <= bulletCount; i++)
      {
        var bulletInstance = Instantiate(bullet);
        IBullet bulletComponent = bulletInstance.GetComponent<IBullet>();
        bulletInstance.transform.position = transform.position + transform.right;
        bulletInstance.transform.rotation = transform.rotation;
        bulletComponent.SetDamage(damage);
        
        // Generate random direction to each bullet
        var deviation = Quaternion.Euler(0f, 0f, Random.Range(-shotAngleDeviation, shotAngleDeviation)); 
        var direction =  deviation * weaponDirection;
        bulletComponent.SetDirection(direction);
        bulletComponent.SetSpeed(speed + Random.Range(-bulletSpeedDeviation, bulletSpeedDeviation));
        bulletInstance.transform.rotation *= deviation;
      }
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