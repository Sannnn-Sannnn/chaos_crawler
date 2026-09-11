using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;

public class WeaponController : MonoBehaviour
{
    [Header("Inputs & Listeners")] [SerializeField]
    private InputActionReference mouseClickListener;

    [SerializeField] private MouseListener mouseListener;

    [SerializeField] private int currentWeaponSlot = 0;

    [Header("Hold Points")] [SerializeField]
    private Transform[] weaponSlots;

    [Header("Weapon Slots")] [SerializeField]
    private WeaponBase[] equipedWeapons;

    [Header("Weapon Slots (UI)")] [SerializeField]
    private WeaponSlot[] uiWeaponSlots;

    private IInteractable _currentInteractable;
    private bool _isHoldingClick = false;

    private void Awake()
    {
        if (mouseListener == null)
        {
            var listenerObject = GameObject.FindGameObjectWithTag("MouseListener");
            if (listenerObject != null)
            {
                mouseListener = listenerObject.GetComponent<MouseListener>();
            }
        }
    }

    private void Start()
    {
        if (equipedWeapons == null || weaponSlots == null) return;

        int count = Mathf.Min(equipedWeapons.Length, weaponSlots.Length);
        for (int i = 0; i < count; i++)
        {
            if (equipedWeapons[i] != null && weaponSlots[i] != null)
            {
                equipedWeapons[i].SetEquipped(weaponSlots[i]);
                uiWeaponSlots[currentWeaponSlot].SetWeaponSprite(weaponSlots[i].GetComponentInChildren<SpriteRenderer>().sprite);
            }
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

        if (mouseListener != null)
        {
            mouseListener.OnWheelScroll += HandleWeaponChange;
        }
    }

    private void OnDisable()
    {
        if (mouseClickListener != null)
        {
            mouseClickListener.action.Disable();
            mouseClickListener.action.started -= OnActionStarted;
            mouseClickListener.action.canceled -= OnActionCanceled;
        }

        if (mouseListener != null)
        {
            mouseListener.OnWheelScroll -= HandleWeaponChange;
        }
    }

    private void Update()
    {
        RotateAndAim();

        if (_isHoldingClick)
        {
            ShootAllWeapons();
        }
    }

    private void RotateAndAim()
    {
        if (mouseListener == null) return;

        if (weaponSlots != null)
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i] == null) continue;

                Vector2 aimDir = mouseListener.GetAimDirection(weaponSlots[i].position);
                float slotAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
                weaponSlots[i].rotation = Quaternion.Euler(0f, 0f, slotAngle);

                if (equipedWeapons != null && i < equipedWeapons.Length && equipedWeapons[i] != null)
                {
                    equipedWeapons[i].FlipSpriteY(aimDir.x < 0);
                }
            }
        }

        Vector2 selfDir = mouseListener.GetAimDirection(transform.position);
        float selfAngle = Mathf.Atan2(selfDir.y, selfDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, selfAngle);
    }

    private void ShootAllWeapons()
    {
        if (equipedWeapons == null || mouseListener == null) return;

        for (int i = 0; i < equipedWeapons.Length; i++)
        {
            if (equipedWeapons[i] == null) continue;

            Transform slot = (weaponSlots != null && i < weaponSlots.Length) ? weaponSlots[i] : transform;
            Vector2 aimDir = slot != null ? mouseListener.GetAimDirection(slot.position) : (Vector2)transform.right;

            equipedWeapons[i].Shoot(aimDir);
        }
    }

    private void OnActionStarted(InputAction.CallbackContext context)
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.Interact(this);
            return;
        }

        _isHoldingClick = true;
        ShootAllWeapons();
    }

    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        _isHoldingClick = false;
    }

    private void HandleWeaponChange(int movement)
    {
        if (equipedWeapons == null || equipedWeapons.Length == 0) return;

        int totalSlots = equipedWeapons.Length;
        currentWeaponSlot = (currentWeaponSlot + movement) % totalSlots;
        if (currentWeaponSlot < 0) currentWeaponSlot += totalSlots;
    }

    public void EquipWeapon(WeaponBase newWeapon)
    {
        if (newWeapon == null || equipedWeapons == null || equipedWeapons.Length == 0) return;

        if (currentWeaponSlot < 0 || currentWeaponSlot >= equipedWeapons.Length)
        {
            currentWeaponSlot = 0;
        }

        Transform targetSlot = (weaponSlots != null && currentWeaponSlot < weaponSlots.Length)
            ? weaponSlots[currentWeaponSlot]
            : transform;

        if (equipedWeapons[currentWeaponSlot] != null)
        {
            equipedWeapons[currentWeaponSlot].SetDropped();
        }

        equipedWeapons[currentWeaponSlot] = newWeapon;
        equipedWeapons[currentWeaponSlot].SetEquipped(targetSlot);
        _currentInteractable = null;
        
        uiWeaponSlots[currentWeaponSlot].SetWeaponSprite(newWeapon.GetComponentInChildren<SpriteRenderer>().sprite);
    }

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