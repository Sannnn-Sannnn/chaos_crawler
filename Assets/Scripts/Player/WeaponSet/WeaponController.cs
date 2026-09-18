using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Weapon;

public class WeaponController : MonoBehaviour
{

    [SerializeField] private WeaponRenderer weaponRenderer;
    
    [Header("Inputs & Listeners")] [SerializeField]
    private InputActionReference mouseClickListener;

    [SerializeField] private MouseListener mouseListener;

    [Header("Hold Points")] [SerializeField]
    private Transform[] weaponSlots;

    [Header("Weapon Slots")] [SerializeField]
    private WeaponBase[] equippedWeapons;
    [SerializeField] private int currentWeaponSlot = 0;
    [SerializeField] private int fixedWeaponSlot = 0;

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
        if (equippedWeapons == null || weaponSlots == null) return;

        if (equippedWeapons.Length < 2 && fixedWeaponSlot >= 0)
        {
            Debug.Log("There must be at least 2 weapon slot or no fixed weapon slot");
            Destroy(gameObject);
        }
        
        int count = Mathf.Min(equippedWeapons.Length, weaponSlots.Length);
        for (int i = 0; i < count; i++)
        {
            var weapon = equippedWeapons[i];
            if (weapon != null && weaponSlots[i] != null)
            {
                equippedWeapons[i].SetEquipped(weaponSlots[i]);
                weaponRenderer.RenderWeapon(i, weapon);
            }
            else
            {
                weaponRenderer.RenderWeapon(i, null);
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
    
    // Attack

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

                if (equippedWeapons != null && i < equippedWeapons.Length && equippedWeapons[i] != null)
                {
                    equippedWeapons[i].FlipSpriteY(aimDir.x < 0);
                }
            }
        }

        Vector2 selfDir = mouseListener.GetAimDirection(transform.position);
        float selfAngle = Mathf.Atan2(selfDir.y, selfDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, selfAngle);
    }

    private void ShootAllWeapons()
    {
        if (equippedWeapons == null || mouseListener == null) return;

        for (int i = 0; i < equippedWeapons.Length; i++)
        {
            if (equippedWeapons[i] == null) continue;

            Transform slot = (weaponSlots != null && i < weaponSlots.Length) ? weaponSlots[i] : transform;
            Vector2 aimDir = slot != null ? mouseListener.GetAimDirection(slot.position) : (Vector2)transform.right;

            equippedWeapons[i].Shoot(aimDir);
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

    // Equipped weapons
    
    private void HandleWeaponChange(int movement)
    {
        if (equippedWeapons == null || equippedWeapons.Length == 0) return;

        weaponRenderer.SetSelected(currentWeaponSlot);
        
        int totalSlots = equippedWeapons.Length;
        currentWeaponSlot = (currentWeaponSlot + movement) % totalSlots;
        if (currentWeaponSlot < 0) currentWeaponSlot += totalSlots;
        
        weaponRenderer.SetSelected(currentWeaponSlot);
    }

    public void EquipWeapon(WeaponBase newWeapon)
    {
        if (newWeapon == null || equippedWeapons == null || equippedWeapons.Length == 0) return;

        CheckNextSlot();
        weaponRenderer.SetSelected(currentWeaponSlot);

    Transform targetSlot = (weaponSlots != null && currentWeaponSlot < weaponSlots.Length)
            ? weaponSlots[currentWeaponSlot]
            : transform;

        if (equippedWeapons[currentWeaponSlot] != null)
        {
            equippedWeapons[currentWeaponSlot].SetDropped();
        }

        equippedWeapons[currentWeaponSlot] = newWeapon;
        equippedWeapons[currentWeaponSlot].SetEquipped(targetSlot);
        _currentInteractable = null;

        weaponRenderer.RenderWeapon(currentWeaponSlot, newWeapon);
    }

    private void CheckNextSlot()
    {
        if (currentWeaponSlot < 0 || currentWeaponSlot >= equippedWeapons.Length)
        {
            currentWeaponSlot = 0;
        }

        // Case 1: empty slot 
        if (equippedWeapons[currentWeaponSlot] == null) return;
        // Case 2: on fixed slot
        if (currentWeaponSlot == fixedWeaponSlot)
        {
            currentWeaponSlot++;
            CheckNextSlot();
            return;
        }
        // Case 3: on common occupied slot with empty slots
        for (int i = 0; i < equippedWeapons.Length; i++)
        {
            var weapon = equippedWeapons[i];
            if (weapon == null)
            {
                currentWeaponSlot = i;
                return;
            }
        }
        // Case 4: on common occupied slot with no empty slots
        return;
    }

    // Triggers

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