using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Inputs & Listeners")]
        [SerializeField] private InputActionReference mouseClickListener;
        [SerializeField] private MouseListener mousePositionListener;

        [Header("Punto de Anclaje")]
        [SerializeField] private Transform weaponHoldPoint;

        [Header("Arma Actual")]
        [SerializeField] private WeaponBase currentWeapon;
        
        [Header("Drop Settings")]
        [SerializeField] private float dropForceMagnitude = 4f;

        private IInteractable _currentInteractable;
        private bool _isHoldingClick = false;

        private void Awake()
        {
            // Fallback por si no se asigna en el Inspector
            if (mousePositionListener == null)
            {
                var listenerObject = GameObject.FindGameObjectWithTag("MouseListener");
                if (listenerObject != null)
                {
                    mousePositionListener = listenerObject.GetComponent<MouseListener>();
                }
            }
        }

        private void Start()
        {
            // Si el jugador arranca con un arma preasignada, inicializarla en el holdPoint
            if (currentWeapon != null)
            {
                currentWeapon.SetEquipped(weaponHoldPoint);
            }
        }

        private void OnEnable()
        {
            if (mouseClickListener != null)
            {
                mouseClickListener.action.Enable();
                mouseClickListener.action.started += OnActionStarted;
                mouseClickListener.action.canceled += OnActionCanceled;
            }
        }

        private void OnDisable()
        {
            if (mouseClickListener != null)
            {
                mouseClickListener.action.started -= OnActionStarted;
                mouseClickListener.action.canceled -= OnActionCanceled;
                mouseClickListener.action.Disable();
            }
        }

        private void Update()
        {
            RotateAndAim();

            // Disparo continuo/automático mientras se mantenga presionado
            if (_isHoldingClick && currentWeapon != null)
            {
                Vector2 aimDirection = mousePositionListener != null 
                    ? mousePositionListener.GetAimDirection(weaponHoldPoint.position) 
                    : (Vector2)weaponHoldPoint.right;

                currentWeapon.Shoot(aimDirection);
            }
        }

        private void RotateAndAim()
        {
            if (mousePositionListener == null || weaponHoldPoint == null) return;

            // 1. Obtener dirección hacia el mouse
            Vector2 mouseDirection = mousePositionListener.GetAimDirection(weaponHoldPoint.position);

            // 2. Rotar el WeaponHoldPoint en el eje Z
            float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
            weaponHoldPoint.rotation = Quaternion.Euler(0f, 0f, angle);

            // 3. Invertir visualmente el sprite del arma si apunta a la izquierda
            if (currentWeapon != null)
            {
                currentWeapon.FlipSpriteY(mouseDirection.x < 0);
            }
        }

        private void OnActionStarted(InputAction.CallbackContext context)
        {
            // PRIORIDAD 1: Si hay un arma/objeto en el suelo, interactuar
            if (_currentInteractable != null)
            {
                _currentInteractable.Interact(this);
                return;
            }

            // PRIORIDAD 2: Disparar arma equipada
            _isHoldingClick = true;
            if (currentWeapon != null && mousePositionListener != null)
            {
                Vector2 aimDirection = mousePositionListener.GetAimDirection(weaponHoldPoint.position);
                currentWeapon.Shoot(aimDirection);
            }
        }

        private void OnActionCanceled(InputAction.CallbackContext context)
        {
            _isHoldingClick = false;
        }

        // --- GESTIÓN DE EQUIPAMIENTO ---
        public void EquipWeapon(WeaponBase newWeapon)
        {
            if (newWeapon == null) return;

            if (currentWeapon != null)
            {
                Vector2 dropDirection = weaponHoldPoint != null ? (Vector2)weaponHoldPoint.right : Vector2.right;
                currentWeapon.SetDropped(dropDirection * dropForceMagnitude);
            }

            currentWeapon = newWeapon;
            currentWeapon.SetEquipped(weaponHoldPoint);
            _currentInteractable = null;
        }
        
        // --- DETECCIÓN DE INTERACTUABLES EN EL SUELO (2D Triggers) ---
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                _currentInteractable = interactable;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                if (_currentInteractable == interactable)
                {
                    _currentInteractable = null;
                }
            }
        }
    }
}